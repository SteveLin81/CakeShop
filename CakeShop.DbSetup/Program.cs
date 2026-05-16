using System.Security.Cryptography;
using System.Text;
using Npgsql;

const string Host      = "127.0.0.1";
const int    Port      = 5432;
const string AppUser   = "testdb0101";
const string AppPass   = "testdb0101";
const string DbName    = "TESTDB";

Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine("  CakeShop 資料庫初始化工具");
Console.WriteLine("═══════════════════════════════════════════\n");

// ── Step 1：嘗試以超級使用者連線（建立帳號與資料庫）────────────────
// 依序嘗試常見的超級使用者帳密組合
var superCandidates = new[]
{
    (User: "postgres",   Pass: ""),
    (User: "postgres",   Pass: "postgres"),
    (User: "postgres",   Pass: "admin"),
    (User: "postgres",   Pass: "password"),
    (User: AppUser,      Pass: AppPass),     // 若帳號已存在直接用
};

NpgsqlConnection? adminConn = null;
string? superUser = null;

foreach (var (u, p) in superCandidates)
{
    try
    {
        var cs = $"Host={Host};Port={Port};Username={u};Password={p};Database=postgres;Timeout=5";
        var c = new NpgsqlConnection(cs);
        await c.OpenAsync();
        adminConn = c;
        superUser = u;
        Console.WriteLine($"✔ 以帳號 '{u}' 連線至 PostgreSQL 成功");
        break;
    }
    catch { }
}

if (adminConn is null)
{
    Console.WriteLine("✗ 無法連線至 PostgreSQL（嘗試帳號: postgres / testdb0101）");
    Console.Write("請輸入 PostgreSQL 超級使用者密碼（postgres）: ");
    var manualPass = Console.ReadLine() ?? "";
    try
    {
        var cs = $"Host={Host};Port={Port};Username=postgres;Password={manualPass};Database=postgres;Timeout=5";
        adminConn = new NpgsqlConnection(cs);
        await adminConn.OpenAsync();
        superUser = "postgres";
        Console.WriteLine("✔ 連線成功");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"✗ 連線失敗：{ex.Message}");
        return;
    }
}

await using (adminConn)
{
    // 建立 role testdb0101（若不存在）
    if (superUser != AppUser)
    {
        var roleExists = await Scalar(adminConn,
            $"SELECT 1 FROM pg_roles WHERE rolname='{AppUser}'");
        if (roleExists is null)
        {
            await Execute(adminConn,
                $"CREATE ROLE \"{AppUser}\" LOGIN PASSWORD '{AppPass}';");
            Console.WriteLine($"✔ 使用者 '{AppUser}' 建立完成");
        }
        else
        {
            await Execute(adminConn,
                $"ALTER ROLE \"{AppUser}\" WITH LOGIN PASSWORD '{AppPass}';");
            Console.WriteLine($"ℹ  使用者 '{AppUser}' 已存在，密碼已更新");
        }
    }

    // 建立 TESTDB 資料庫（若不存在）
    var dbExists = await Scalar(adminConn,
        $"SELECT 1 FROM pg_database WHERE LOWER(datname)='{DbName.ToLower()}'");
    if (dbExists is null)
    {
        await Execute(adminConn,
            $"CREATE DATABASE \"{DbName}\" OWNER \"{AppUser}\" ENCODING 'UTF8';");
        Console.WriteLine($"✔ 資料庫 '{DbName}' 建立完成");
    }
    else
    {
        Console.WriteLine($"ℹ  資料庫 '{DbName}' 已存在");
    }

    // 授予權限
    await Execute(adminConn, $"GRANT ALL PRIVILEGES ON DATABASE \"{DbName}\" TO \"{AppUser}\";");
    Console.WriteLine($"✔ 已授予 '{AppUser}' 完整權限");
}

// ── Step 2：以 AppUser 連線至 TESTDB，建立 schema ───────────────────
Console.WriteLine($"\n連線至 {DbName} 建立資料表...");
var connStr = $"Host={Host};Port={Port};Username={AppUser};Password={AppPass};Database={DbName}";
await using var db = new NpgsqlConnection(connStr);
await db.OpenAsync();
Console.WriteLine($"✔ 已連線至 {DbName}（使用者: {AppUser}）\n");

await Execute(db, "CREATE EXTENSION IF NOT EXISTS pgcrypto;");

// categories
await Execute(db, """
    CREATE TABLE IF NOT EXISTS categories (
        id           SERIAL       PRIMARY KEY,
        name         VARCHAR(50)  NOT NULL,
        name_en      VARCHAR(50),
        name_ja      VARCHAR(50),
        name_zh_cn   VARCHAR(50),
        created_at   TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
        created_by   VARCHAR(100) NOT NULL DEFAULT 'admin',
        updated_at   TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
        updated_by   VARCHAR(100) NOT NULL DEFAULT 'admin',
        update_count INTEGER      NOT NULL DEFAULT 0
    );
""");

// products
await Execute(db, """
    CREATE TABLE IF NOT EXISTS products (
        id                SERIAL        PRIMARY KEY,
        name              VARCHAR(100)  NOT NULL,
        name_en           VARCHAR(100),
        name_ja           VARCHAR(100),
        name_zh_cn        VARCHAR(100),
        description       TEXT,
        description_en    TEXT,
        description_ja    TEXT,
        description_zh_cn TEXT,
        price             NUMERIC(10,2) NOT NULL,
        image_url         VARCHAR(500),
        category_id       INTEGER       REFERENCES categories(id),
        is_available      BOOLEAN       NOT NULL DEFAULT TRUE,
        created_at        TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
        created_by        VARCHAR(100)  NOT NULL DEFAULT 'admin',
        updated_at        TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
        updated_by        VARCHAR(100)  NOT NULL DEFAULT 'admin',
        update_count      INTEGER       NOT NULL DEFAULT 0
    );
""");

// users
await Execute(db, """
    CREATE TABLE IF NOT EXISTS users (
        id            SERIAL       PRIMARY KEY,
        username      VARCHAR(50)  UNIQUE NOT NULL,
        password_hash VARCHAR(255) NOT NULL,
        email         VARCHAR(100),
        created_at    TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
        created_by    VARCHAR(100) NOT NULL DEFAULT 'admin',
        updated_at    TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
        updated_by    VARCHAR(100) NOT NULL DEFAULT 'admin',
        update_count  INTEGER      NOT NULL DEFAULT 0
    );
""");

// cart_items
await Execute(db, """
    CREATE TABLE IF NOT EXISTS cart_items (
        id           SERIAL       PRIMARY KEY,
        session_id   VARCHAR(100) NOT NULL,
        product_id   INTEGER      REFERENCES products(id) ON DELETE CASCADE,
        quantity     INTEGER      NOT NULL DEFAULT 1 CHECK (quantity > 0),
        created_at   TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
        created_by   VARCHAR(100) NOT NULL DEFAULT 'admin',
        updated_at   TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
        updated_by   VARCHAR(100) NOT NULL DEFAULT 'admin',
        update_count INTEGER      NOT NULL DEFAULT 0
    );
    CREATE INDEX IF NOT EXISTS idx_cart_session ON cart_items(session_id);
""");

// announcements
await Execute(db, """
    CREATE TABLE IF NOT EXISTS announcements (
        id            SERIAL       PRIMARY KEY,
        content       TEXT         NOT NULL,
        content_en    TEXT,
        content_ja    TEXT,
        content_zh_cn TEXT,
        is_active     BOOLEAN      NOT NULL DEFAULT TRUE,
        created_at    TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
        created_by    VARCHAR(100) NOT NULL DEFAULT 'admin',
        updated_at    TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
        updated_by    VARCHAR(100) NOT NULL DEFAULT 'admin',
        update_count  INTEGER      NOT NULL DEFAULT 0
    );
""");

// b2e_roles（後台角色，必須在 b2e_users 之前建立）
await Execute(db, """
    CREATE TABLE IF NOT EXISTS b2e_roles (
        id           SERIAL       PRIMARY KEY,
        name         VARCHAR(50)  UNIQUE NOT NULL,
        description  VARCHAR(200) NOT NULL DEFAULT '',
        permissions  TEXT         NOT NULL DEFAULT '[]',
        created_at   TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
        created_by   VARCHAR(100) NOT NULL DEFAULT 'system',
        updated_at   TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
        updated_by   VARCHAR(100) NOT NULL DEFAULT 'system',
        update_count INTEGER      NOT NULL DEFAULT 0
    );
""");

// b2e_users（後台管理員帳號，獨立於 B2C users）
await Execute(db, """
    CREATE TABLE IF NOT EXISTS b2e_users (
        id                   SERIAL       PRIMARY KEY,
        username             VARCHAR(50)  UNIQUE NOT NULL,
        password_hash        VARCHAR(255) NOT NULL,
        email                VARCHAR(100) NOT NULL DEFAULT '',
        role_id              INTEGER      REFERENCES b2e_roles(id) ON DELETE SET NULL,
        must_change_password BOOLEAN      NOT NULL DEFAULT FALSE,
        created_at           TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
        created_by           VARCHAR(100) NOT NULL DEFAULT 'system',
        updated_at           TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
        updated_by           VARCHAR(100) NOT NULL DEFAULT 'system',
        update_count         INTEGER      NOT NULL DEFAULT 0
    );
""");

Console.WriteLine("✔ 7 張資料表建立完成（含 b2e_roles / b2e_users）");

// ── Step 2b：新增多語系欄位（IF NOT EXISTS，安全重複執行）─────────────
await Execute(db, """
    ALTER TABLE categories
        ADD COLUMN IF NOT EXISTS name_th VARCHAR(50) NOT NULL DEFAULT '',
        ADD COLUMN IF NOT EXISTS name_ko VARCHAR(50) NOT NULL DEFAULT '',
        ADD COLUMN IF NOT EXISTS name_vi VARCHAR(50) NOT NULL DEFAULT '',
        ADD COLUMN IF NOT EXISTS name_ms VARCHAR(50) NOT NULL DEFAULT '';
    ALTER TABLE products
        ADD COLUMN IF NOT EXISTS name_th        VARCHAR(100) NOT NULL DEFAULT '',
        ADD COLUMN IF NOT EXISTS name_ko        VARCHAR(100) NOT NULL DEFAULT '',
        ADD COLUMN IF NOT EXISTS name_vi        VARCHAR(100) NOT NULL DEFAULT '',
        ADD COLUMN IF NOT EXISTS name_ms        VARCHAR(100) NOT NULL DEFAULT '',
        ADD COLUMN IF NOT EXISTS description_th TEXT NOT NULL DEFAULT '',
        ADD COLUMN IF NOT EXISTS description_ko TEXT NOT NULL DEFAULT '',
        ADD COLUMN IF NOT EXISTS description_vi TEXT NOT NULL DEFAULT '',
        ADD COLUMN IF NOT EXISTS description_ms TEXT NOT NULL DEFAULT '';
    ALTER TABLE announcements
        ADD COLUMN IF NOT EXISTS content_th TEXT,
        ADD COLUMN IF NOT EXISTS content_ko TEXT,
        ADD COLUMN IF NOT EXISTS content_vi TEXT,
        ADD COLUMN IF NOT EXISTS content_ms TEXT;
""");
Console.WriteLine("✔ 多語系欄位就緒");

// ── Step 2b-2：新增 is_featured 欄位 ────────────────────────────────
await Execute(db, """
    ALTER TABLE products
        ADD COLUMN IF NOT EXISTS is_featured BOOLEAN NOT NULL DEFAULT FALSE;
""");
Console.WriteLine("✔ is_featured 欄位就緒");

// ── Step 2c：新增稽核欄位（IF NOT EXISTS，安全重複執行）────────────────
await Execute(db, """
    ALTER TABLE categories
        ADD COLUMN IF NOT EXISTS created_at   TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
        ADD COLUMN IF NOT EXISTS created_by   VARCHAR(100) NOT NULL DEFAULT 'admin',
        ADD COLUMN IF NOT EXISTS updated_at   TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
        ADD COLUMN IF NOT EXISTS updated_by   VARCHAR(100) NOT NULL DEFAULT 'admin',
        ADD COLUMN IF NOT EXISTS update_count INTEGER      NOT NULL DEFAULT 0;
    ALTER TABLE products
        ADD COLUMN IF NOT EXISTS created_by   VARCHAR(100) NOT NULL DEFAULT 'admin',
        ADD COLUMN IF NOT EXISTS updated_at   TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
        ADD COLUMN IF NOT EXISTS updated_by   VARCHAR(100) NOT NULL DEFAULT 'admin',
        ADD COLUMN IF NOT EXISTS update_count INTEGER      NOT NULL DEFAULT 0;
    ALTER TABLE users
        ADD COLUMN IF NOT EXISTS created_by   VARCHAR(100) NOT NULL DEFAULT 'admin',
        ADD COLUMN IF NOT EXISTS updated_at   TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
        ADD COLUMN IF NOT EXISTS updated_by   VARCHAR(100) NOT NULL DEFAULT 'admin',
        ADD COLUMN IF NOT EXISTS update_count INTEGER      NOT NULL DEFAULT 0;
    ALTER TABLE cart_items
        ADD COLUMN IF NOT EXISTS created_by   VARCHAR(100) NOT NULL DEFAULT 'admin',
        ADD COLUMN IF NOT EXISTS updated_by   VARCHAR(100) NOT NULL DEFAULT 'admin',
        ADD COLUMN IF NOT EXISTS update_count INTEGER      NOT NULL DEFAULT 0;
    ALTER TABLE announcements
        ADD COLUMN IF NOT EXISTS created_by   VARCHAR(100) NOT NULL DEFAULT 'admin',
        ADD COLUMN IF NOT EXISTS updated_by   VARCHAR(100) NOT NULL DEFAULT 'admin',
        ADD COLUMN IF NOT EXISTS update_count INTEGER      NOT NULL DEFAULT 0;
""");
Console.WriteLine("✔ 稽核欄位就緒（created_by / updated_at / updated_by / update_count）\n正在插入種子資料...");

// ── Step 3：種子資料 ────────────────────────────────────────────────
await Execute(db, """
    INSERT INTO categories (id,name,name_en,name_ja,name_zh_cn,name_th,name_ko,name_vi,name_ms) VALUES
        (1,'巧克力','Chocolate', 'チョコレート','巧克力', 'ช็อกโกแลต','초콜릿',   'Sô Cô La', 'Coklat'),
        (2,'水果',   'Fruit',     'フルーツ',    '水果',   'ผลไม้',     '과일',      'Trái Cây', 'Buah-buahan'),
        (3,'日式',   'Japanese',  '和菓子風',    '日式',   'ญี่ปุ่น',   '일본식',   'Nhật Bản', 'Jepun'),
        (4,'起司',   'Cheesecake','チーズケーキ', '奶酪',  'ชีสเค้ก',   '치즈케이크','Bánh Phô Mai','Kek Keju'),
        (5,'經典',   'Classic',   'クラシック',  '经典',   'คลาสสิก',   '클래식',   'Cổ Điển',  'Klasik'),
        (6,'柑橘',   'Citrus',    'シトラス',    '柑橘',   'ซิตรัส',    '시트러스', 'Trái Có Múi','Sitrus')
    ON CONFLICT (id) DO UPDATE SET
        name_th = EXCLUDED.name_th, name_ko = EXCLUDED.name_ko,
        name_vi = EXCLUDED.name_vi, name_ms = EXCLUDED.name_ms;
    SELECT setval('categories_id_seq',(SELECT MAX(id) FROM categories));
""");
Console.WriteLine("  ✔ categories（6 筆）");

await Execute(db, """
    INSERT INTO products(id,name,name_en,name_ja,name_zh_cn,name_th,name_ko,name_vi,name_ms,
        description,description_en,description_ja,description_zh_cn,
        description_th,description_ko,description_vi,description_ms,
        price,image_url,category_id) VALUES
    (1,'巧克力熔岩蛋糕','Chocolate Lava Cake','チョコレートラバーケーキ','巧克力熔岩蛋糕',
     'เค้กช็อกโกแลตลาวา','초콜릿 용암 케이크','Bánh Chocolate Lava','Kek Coklat Lava',
     '濃郁的巧克力外殼，內部流動著滑順的巧克力漿，每一口都是幸福的滋味。',
     'Rich chocolate shell with a smooth flowing center, pure bliss in every bite.',
     '濃厚なチョコレートシェルの中に、とろけるチョコレートが流れ出す幸せの一品。',
     '浓郁的巧克力外壳，内部流动着顺滑的巧克力浆，每一口都是幸福的滋味。',
     'เปลือกช็อกโกแลตนอกกรอบ ข้างในเต็มไปด้วยซอสช็อกโกแลตเหลวหอมหวาน',
     '겉은 바삭하고 안에는 부드러운 초콜릿이 흘러내리는 행복한 케이크',
     'Vỏ ngoài giòn tan, bên trong là chocolate lỏng ngọt ngào, từng miếng là hạnh phúc',
     'Luar rangup, dalam penuh coklat cair yang lazat, setiap suapan penuh kegembiraan',
     280,'https://images.unsplash.com/photo-1578985545062-69928b1d9587?w=480&h=360&fit=crop&auto=format',1),
    (2,'草莓鮮奶油蛋糕','Strawberry Fresh Cream Cake','ストロベリーショートケーキ','草莓鲜奶油蛋糕',
     'เค้กครีมสดสตรอเบอร์รี่','딸기 생크림 케이크','Bánh Kem Dâu Tây Tươi','Kek Krim Segar Strawberi',
     '精選新鮮草莓搭配輕盈鮮奶油，清爽甜美的口感令人難以忘懷。',
     'Fresh strawberries with light whipped cream, a refreshingly sweet classic.',
     '新鮮なイチゴと軽やかな生クリームが絶妙にマッチした定番ケーキ。',
     '精选新鲜草莓搭配轻盈鲜奶油，清爽甜美的口感令人难以忘怀。',
     'สตรอเบอร์รี่สดกับวิปครีมเบา หวานสดชื่น น่าลิ้มรสยิ่งนัก',
     '신선한 딸기와 부드러운 생크림의 조화, 상큼하고 달콤한 클래식 케이크',
     'Dâu tây tươi kết hợp kem nhẹ nhàng, ngọt ngào và thanh mát',
     'Strawberi segar dengan krim ringan, manis dan segar tiada tandingan',
     350,'https://images.unsplash.com/photo-1565958011703-44f9829ba187?w=480&h=360&fit=crop&auto=format',2),
    (3,'抹茶紅豆蛋糕','Matcha Red Bean Cake','抹茶小豆ケーキ','抹茶红豆蛋糕',
     'เค้กชาเขียวถั่วแดง','말차 팥 케이크','Bánh Trà Xanh Đậu Đỏ','Kek Teh Hijau Kacang Merah',
     '精選宇治抹茶與細緻紅豆泥完美融合，帶來獨特的日式風味體驗。',
     'Premium Uji matcha blended with smooth red bean paste, a unique Japanese experience.',
     '厳選された宇治抹茶と丁寧に炊いた小豆あんが調和した和の一品。',
     '精选宇治抹茶与细腻红豆泥完美融合，带来独特的日式风味体验。',
     'ชาเขียวอุจิคัดพิเศษผสมถั่วแดงนุ่ม รสชาติญี่ปุ่นแท้',
     '고급 우지 말차와 부드러운 팥이 완벽하게 어우러진 일본풍 케이크',
     'Trà xanh Uji thượng hạng hòa quyện với đậu đỏ mịn màng, hương vị Nhật Bản độc đáo',
     'Teh hijau Uji premium berpadu dengan pes kacang merah yang lembut, rasa Jepun tulen',
     320,'https://images.unsplash.com/photo-1556679343-c7306c1976bc?w=480&h=360&fit=crop&auto=format',3),
    (4,'芒果慕斯蛋糕','Mango Mousse Cake','マンゴームースケーキ','芒果慕斯蛋糕',
     'เค้กมูสมะม่วง','망고 무스 케이크','Bánh Mousse Xoài','Kek Mousse Mangga',
     '選用台灣愛文芒果製作，綿密慕斯搭配新鮮芒果丁，夏日最清爽的享受。',
     'Made with Irwin mangoes, silky mousse with fresh mango chunks — perfect summer treat.',
     '台湾産アーウィンマンゴー使用。なめらかなムースとフレッシュマンゴーの夏の逸品。',
     '选用台湾爱文芒果制作，绵密慕斯搭配新鲜芒果丁，夏日最清爽的享受。',
     'มะม่วงไต้หวันคัดพิเศษ มูสนุ่มลิ้นกับชิ้นมะม่วงสด ความสดชื่นของฤดูร้อน',
     '대만 망고로 만든 실키한 무스와 신선한 망고 조각, 여름의 청량한 디저트',
     'Xoài Đài Loan tuyển chọn, mousse mịn màng với miếng xoài tươi, thưởng thức mùa hè',
     'Mangga Taiwan terpilih, mousse lembut dengan kepingan mangga segar, kesegaran musim panas',
     380,'https://images.unsplash.com/photo-1587314168485-3236d6710814?w=480&h=360&fit=crop&auto=format',2),
    (5,'藍莓起司蛋糕','Blueberry Cheesecake','ブルーベリーチーズケーキ','蓝莓奶酪蛋糕',
     'ชีสเค้กบลูเบอร์รี่','블루베리 치즈케이크','Bánh Phô Mai Việt Quất','Kek Keju Blueberry',
     '濃郁奶油起司底層，鋪上新鮮藍莓果醬，酸甜平衡的完美滋味。',
     'Rich cream cheese base topped with fresh blueberry compote, perfectly balanced.',
     '濃厚なクリームチーズの上に新鮮なブルーベリーソースをのせた酸甘の逸品。',
     '浓郁奶油奶酪底层，铺上新鲜蓝莓果酱，酸甜平衡的完美滋味。',
     'ฐานชีสครีมเข้มข้น ราดซอสบลูเบอร์รี่สด รสเปรี้ยวอมหวานสมดุลลงตัว',
     '진한 크림치즈 베이스에 신선한 블루베리 컴포트, 완벽한 달콤새콤 균형',
     'Lớp phô mai kem đậm đà, phủ mứt việt quất tươi, vị chua ngọt hài hòa hoàn hảo',
     'Lapisan krim keju yang kaya, ditutup kompot blueberry segar, rasa manis masam yang seimbang',
     420,'https://images.unsplash.com/photo-1571115177098-24ec42ed204d?w=480&h=360&fit=crop&auto=format',4),
    (6,'黑森林蛋糕','Black Forest Cake','シュヴァルツヴェルダー','黑森林蛋糕',
     'เค้กป่าดำ','블랙 포레스트 케이크','Bánh Black Forest','Kek Hutan Hitam',
     '德式傳統經典，黑巧克力蛋糕搭配酸甜酒漬車厘子與鮮奶油，層次豐富迷人。',
     'German classic with dark chocolate cake, kirsch-soaked cherries and cream layers.',
     'ダークチョコレートとキルシュ漬けチェリー、生クリームが重なるドイツの定番。',
     '德式传统经典，黑巧克力蛋糕搭配酸甜酒渍车厘子与鲜奶油，层次丰富迷人。',
     'สูตรดั้งเดิมเยอรมัน เค้กช็อกโกแลตดำกับเชอร์รี่แช่ไวน์และวิปครีม หลายชั้น',
     '독일 전통 클래식, 다크초콜릿 케이크에 체리와 생크림이 층층이 쌓인 풍성한 맛',
     'Cổ điển Đức, bánh socola đen kết hợp anh đào ngâm rượu và kem tươi, nhiều lớp quyến rũ',
     'Klasik Jerman, kek coklat gelap dengan ceri rendaman kirsch dan krim, berlapis-lapis',
     450,'https://images.unsplash.com/photo-1464195244916-405fa0a82545?w=480&h=360&fit=crop&auto=format',1),
    (7,'提拉米蘇','Tiramisu','ティラミス','提拉米苏',
     'ทีรามิสุ','티라미수','Tiramisu','Tiramisu',
     '義式經典甜點，手指餅乾浸泡濃縮咖啡，搭配馬斯卡邦起司，回味無窮。',
     'Italian classic with espresso-soaked ladyfingers and velvety mascarpone cream.',
     'エスプレッソに浸したサヴォイアルディとマスカルポーネクリームのイタリア名菓。',
     '意式经典甜点，手指饼干浸泡浓缩咖啡，搭配马斯卡彭奶酪，回味无穷。',
     'ขนมอิตาลีคลาสสิก บิสกิตชุ่มเอสเปรสโซ กับมาสคาร์โปเน่ครีมเนียนนุ่ม',
     '이탈리아 클래식 디저트, 에스프레소에 적신 레이디핑거와 마스카르포네 크림',
     'Bánh Ý kinh điển, bánh quy ngâm espresso kết hợp kem mascarpone mịn màng',
     'Pencuci mulut Itali klasik, biskut direndam espresso dengan krim mascarpone yang lembut',
     360,'https://images.unsplash.com/photo-1542124948-dc391252a940?w=480&h=360&fit=crop&auto=format',5),
    (8,'紅絲絨蛋糕','Red Velvet Cake','レッドベルベットケーキ','红丝绒蛋糕',
     'เค้กกำมะหยี่แดง','레드 벨벳 케이크','Bánh Red Velvet','Kek Baldu Merah',
     '美式經典紅絲絨，絲滑奶油起司霜配上鮮豔紅色蛋糕體，視覺與味覺的雙重享受。',
     'American classic with vibrant red cake and silky cream cheese frosting.',
     '鮮やかな赤いスポンジとなめらかなクリームチーズフロスティングの美式定番。',
     '美式经典红丝绒，丝滑奶油奶酪霜配上鲜艳红色蛋糕体，视觉与味觉的双重享受。',
     'เค้กสีแดงสดใสสไตล์อเมริกัน กับฟรอสติ้งชีสครีมเนียน ทั้งตาและปากอิ่มเอม',
     '미국 클래식 레드벨벳, 선명한 빨간 케이크에 부드러운 크림치즈 프로스팅',
     'Bánh Mỹ kinh điển, lớp bánh đỏ tươi với kem phô mai mịn màng, vừa đẹp vừa ngon',
     'Klasik Amerika, kek merah terang dengan lapisan krim keju yang lembut',
     400,'https://images.unsplash.com/photo-1535141192574-5d4897c12636?w=480&h=360&fit=crop&auto=format',5),
    (9,'檸檬磅蛋糕','Lemon Pound Cake','レモンパウンドケーキ','柠檬磅蛋糕',
     'เค้กปอนด์เลมอน','레몬 파운드 케이크','Bánh Bơ Chanh','Kek Paun Lemon',
     '新鮮檸檬汁與檸檬皮充分融入蛋糕體中，清爽酸甜，搭配糖霜更添風味。',
     'Fresh lemon juice and zest infused throughout, light and tangy with a sugar glaze.',
     '新鮮なレモン果汁と皮をたっぷり使った爽やかな酸甘のパウンドケーキ。',
     '新鲜柠檬汁与柠檬皮充分融入蛋糕体中，清爽酸甜，搭配糖霜更添风味。',
     'น้ำมะนาวสดและผิวมะนาวผสานในเนื้อเค้ก เปรี้ยวหอมสดชื่น คู่กับกลาซูร์',
     '신선한 레몬 즙과 레몬 제스트가 가득한 상큼한 파운드 케이크',
     'Nước chanh và vỏ chanh tươi thấm đều vào bánh, chua ngọt thanh mát với lớp đường',
     'Jus lemon segar dan kulit lemon menyerap ke dalam kek, masam manis menyegarkan',
     290,'https://images.unsplash.com/photo-1519915028421-c6c1cb6c7a8b?w=480&h=360&fit=crop&auto=format',6),
    (10,'焦糖蘋果塔','Caramel Apple Tart','キャラメルアップルタルト','焦糖苹果塔',
     'ทาร์ตแอปเปิลคาราเมล','캐러멜 사과 타르트','Bánh Táo Caramel','Tart Epal Karamel',
     '酥脆塔皮搭配焦糖蘋果，淋上香濃焦糖醬，法式風情盡在其中。',
     'Crispy tart shell with caramelized apples and a rich caramel drizzle, French elegance.',
     'サクサクのタルト生地にキャラメルアップルを並べ、濃厚なキャラメルソースを添えた逸品。',
     '酥脆塔皮搭配焦糖苹果，淋上香浓焦糖酱，法式风情尽在其中。',
     'แป้งทาร์ตกรอบกับแอปเปิ้ลคาราเมล ราดซอสคาราเมลเข้มข้น สไตล์ฝรั่งเศส',
     '바삭한 타르트 쉘에 캐러멜 애플과 진한 캐러멜 소스, 프렌치 엘레강스',
     'Đế bánh giòn rụm với táo caramel và sốt caramel đậm đà, phong cách Pháp tinh tế',
     'Alas tart rangup dengan epal karamel dan sos karamel pekat, keanggunan Perancis',
     340,'https://images.unsplash.com/photo-1562440499-64c9a111f713?w=480&h=360&fit=crop&auto=format',2)
    ON CONFLICT (id) DO UPDATE SET
        name_th = EXCLUDED.name_th, name_ko = EXCLUDED.name_ko,
        name_vi = EXCLUDED.name_vi, name_ms = EXCLUDED.name_ms,
        description_th = EXCLUDED.description_th, description_ko = EXCLUDED.description_ko,
        description_vi = EXCLUDED.description_vi, description_ms = EXCLUDED.description_ms;
    SELECT setval('products_id_seq',(SELECT MAX(id) FROM products));
""");
Console.WriteLine("  ✔ products（10 筆，8 語系）");

var hash = ComputeHash("test");
await Execute(db, $"""
    INSERT INTO users (username, password_hash, email)
    VALUES ('test', '{hash}', 'test@cakeshop.com')
    ON CONFLICT (username) DO NOTHING;
""");
Console.WriteLine("  ✔ users（test / test）");

var b2eHash = ComputeHash("testb2e");
// 先插入預設角色
await Execute(db, $"""
    INSERT INTO b2e_roles (name, description, permissions, created_by, updated_by) VALUES
        ('admin',   '系統管理員，擁有全部權限',                '["dashboard","products","categories","announcements","members","homepage","roles","admins"]', 'system', 'system'),
        ('商品部門', '商品管理員，管理商品、分類與首頁設定',    '["dashboard","products","categories","homepage"]',                                          'system', 'system'),
        ('客服部門', '客服人員，管理前台會員與公告',            '["dashboard","announcements","members"]',                                                    'system', 'system')
    ON CONFLICT (name) DO NOTHING;
""");
Console.WriteLine("  ✔ b2e_roles（admin / 商品部門 / 客服部門）");

// 先確保欄位存在（idempotent），再做 INSERT/UPDATE
await Execute(db, """
    ALTER TABLE b2e_users ADD COLUMN IF NOT EXISTS role_id INT REFERENCES b2e_roles(id) ON DELETE SET NULL;
    ALTER TABLE b2e_users ADD COLUMN IF NOT EXISTS must_change_password BOOLEAN NOT NULL DEFAULT FALSE;
    ALTER TABLE b2e_users ADD COLUMN IF NOT EXISTS reset_token VARCHAR(100);
    ALTER TABLE b2e_users ADD COLUMN IF NOT EXISTS reset_token_expires TIMESTAMPTZ;
    ALTER TABLE users ADD COLUMN IF NOT EXISTS reset_token VARCHAR(100);
    ALTER TABLE users ADD COLUMN IF NOT EXISTS reset_token_expires TIMESTAMPTZ;
""");

await Execute(db, $"""
    INSERT INTO b2e_users (username, password_hash, email, role_id, must_change_password, created_by, updated_by)
    SELECT 'testb2e', '{b2eHash}', 'admin@cakeshop.com',
           (SELECT id FROM b2e_roles WHERE name = 'admin'), FALSE, 'system', 'system'
    WHERE NOT EXISTS (SELECT 1 FROM b2e_users WHERE username = 'testb2e');

    UPDATE b2e_users
    SET role_id = (SELECT id FROM b2e_roles WHERE name = 'admin'),
        must_change_password = FALSE
    WHERE username = 'testb2e';
""");
Console.WriteLine("  ✔ b2e_users（testb2e / testb2e，角色：admin）");

await Execute(db, """
    INSERT INTO announcements (content,content_en,content_ja,content_zh_cn,content_th,content_ko,content_vi,content_ms,is_active)
    SELECT '📢 4月30日公休，出貨日期順延一天，敬請見諒。',
           '📢 We are closed on April 30. Orders will be shipped one day later.',
           '📢 4月30日は休業となります。出荷日が1日遅れます。',
           '📢 4月30日公休，发货日期顺延一天，敬请谅解。',
           '📢 หยุดทำการวันที่ 30 เมษายน การจัดส่งเลื่อนออกไป 1 วัน ขออภัยในความไม่สะดวก',
           '📢 4월 30일 휴무입니다. 배송일이 하루 연기됩니다. 양해 부탁드립니다.',
           '📢 Nghỉ ngày 30/4. Ngày giao hàng sẽ bị trễ 1 ngày. Chúng tôi xin lỗi vì sự bất tiện này.',
           '📢 Kami tutup pada 30 April. Penghantaran ditangguhkan sehari. Mohon maaf atas kesulitan ini.',
           TRUE
    WHERE NOT EXISTS (SELECT 1 FROM announcements);
    UPDATE announcements SET
        content_th = '📢 หยุดทำการวันที่ 30 เมษายน การจัดส่งเลื่อนออกไป 1 วัน ขออภัยในความไม่สะดวก',
        content_ko = '📢 4월 30일 휴무입니다. 배송일이 하루 연기됩니다. 양해 부탁드립니다.',
        content_vi = '📢 Nghỉ ngày 30/4. Ngày giao hàng sẽ bị trễ 1 ngày. Chúng tôi xin lỗi vì sự bất tiện này.',
        content_ms = '📢 Kami tutup pada 30 April. Penghantaran ditangguhkan sehari. Mohon maaf atas kesulitan ini.'
    WHERE content_th IS NULL OR content_ko IS NULL;
""");
Console.WriteLine("  ✔ announcements（1 筆）");

// ── Step 4：驗證 ────────────────────────────────────────────────────
Console.WriteLine("\n已建立資料表：");
await using var r = await new NpgsqlCommand(
    "SELECT tablename, pg_size_pretty(pg_total_relation_size(quote_ident(tablename))) AS size " +
    "FROM pg_tables WHERE schemaname='public' ORDER BY tablename", db).ExecuteReaderAsync();
while (await r.ReadAsync())
    Console.WriteLine($"  - {r.GetString(0),-20} {r.GetString(1)}");

Console.WriteLine($"""

✅ 完成！資料庫摘要：
   Host     : {Host}:{Port}
   Database : {DbName}
   Username : {AppUser}
   資料表   : categories, products, users, cart_items, announcements
   種子資料 : 6 分類 / 10 商品 / 1 帳號(test/test) / 1 公告
""");

static async Task Execute(NpgsqlConnection c, string sql)
{
    await using var cmd = new NpgsqlCommand(sql, c);
    await cmd.ExecuteNonQueryAsync();
}
static async Task<object?> Scalar(NpgsqlConnection c, string sql)
{
    await using var cmd = new NpgsqlCommand(sql, c);
    return await cmd.ExecuteScalarAsync();
}
static string ComputeHash(string pwd)
{
    using var sha = SHA256.Create();
    return Convert.ToBase64String(
        sha.ComputeHash(Encoding.UTF8.GetBytes(pwd + "CakeShopPasswordSalt@2024")));
}
