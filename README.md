# 🎂 Sweet Bakes 甜蜜烘焙坊

多語系蛋糕購物網站，後端採 .NET 8 四層架構 + Entity Framework Core 串接 PostgreSQL，前端以 Vue 3 + Element Plus 實作，支援 **8 種語言**切換。

| 語系代碼 | 語言 |
|---------|------|
| `zh-TW` | 繁體中文 |
| `zh-CN` | 简体中文 |
| `en` | English |
| `ja` | 日本語 |
| `th` | ภาษาไทย（泰文） |
| `ko` | 한국어（韓文） |
| `vi` | Tiếng Việt（越南文） |
| `ms` | Bahasa Melayu（馬來文） |

---

## 技術堆疊

| 層次 | 技術 |
|------|------|
| 後端框架 | ASP.NET Core 8 Web API |
| 語言 | C# 12 |
| ORM | Entity Framework Core 8（Npgsql 驅動） |
| 資料庫 | PostgreSQL 9.6+ |
| 前端框架 | Vue 3（CDN）、Element Plus、Vue-i18n |
| 加密 | SHA-256 密碼雜湊 + AES-256-GCM Token |
| API 文件 | Swagger / Swashbuckle |

---

## 專案架構

Solution 依用途分為三個 Solution Folder：

```
CakeShop/
│
├── 📁 _B2C/                                    ← Solution Folder
│   └── EC.B2C/                                 # 網站入口層（輸入 / 呼叫 / 輸出）
│       ├── Controllers/
│       │   ├── AuthController.cs
│       │   ├── ProductController.cs
│       │   ├── CartController.cs
│       │   ├── ContactController.cs
│       │   └── AnnouncementController.cs
│       ├── Program.cs                          # DI 註冊、DbContext、Swagger
│       ├── appsettings.json                    # 含 ConnectionStrings
│       └── wwwroot/                            # 前端靜態頁面
│           ├── index.html / products.html / contact.html
│           ├── css/style.css
│           └── js/i18n.js / api.js
│
├── 📁 Business Libraries/                      ← Solution Folder
│   └── EC.CommonService/                       # 商業邏輯層
│       └── Services/
│           ├── EncryptionService.cs            # SHA-256 雜湊 + AES-256-GCM 加解密
│           ├── AuthService.cs                  # 登入驗證、Token 產生
│           ├── ProductService.cs               # 商品查詢邏輯
│           ├── CartService.cs                  # 購物車操作邏輯
│           ├── ContactService.cs               # 聯絡表單處理
│           └── AnnouncementService.cs          # 置頂公告（從 DB 讀取）
│
├── 📁 Framework/                               ← Solution Folder
│   └── EC.Entities/                            # Domain 實體層（無任何外部相依）
│       └── Models/
│           ├── Product.cs
│           ├── Category.cs
│           ├── User.cs
│           ├── CartItem.cs
│           └── Announcement.cs
│
├── CakeShop.Core/                              # 合約層（DTOs、Interfaces）
│   ├── DTOs/
│   │   ├── ProductDto.cs / CategoryDto.cs
│   │   ├── CartDto.cs
│   │   ├── LoginRequest.cs / LoginResponse.cs
│   │   ├── ContactFormDto.cs
│   │   └── AnnouncementDto.cs
│   └── Interfaces/
│       ├── IEncryptionService.cs
│       ├── IAuthService.cs / IProductService.cs / ICartService.cs
│       ├── IContactService.cs / IAnnouncementService.cs
│       ├── IProductRepository.cs / IUserRepository.cs
│       └── ICartRepository.cs / IAnnouncementRepository.cs
│
├── CakeShop.Infrastructure/                    # 資料存取層（EF Core）
│   ├── Data/
│   │   ├── CakeShopDbContext.cs                # EF Core DbContext
│   │   └── DesignTimeDbContextFactory.cs       # 供 dotnet ef 使用
│   └── Repositories/
│       ├── ProductRepository.cs
│       ├── UserRepository.cs
│       ├── CartRepository.cs
│       └── AnnouncementRepository.cs
│
└── CakeShop.DbSetup/                           # 資料庫初始化工具
    └── Program.cs                              # 建立 DB、資料表、種子資料
```

### 命名空間對應

| 專案 | 命名空間 | 職責 |
|------|---------|------|
| `EC.B2C` | `EC.B2C.Controllers` | HTTP 輸入／輸出，不含商業邏輯 |
| `EC.CommonService` | `EC.CommonService.Services` | 商業邏輯實作 |
| `EC.Entities` | `EC.Entities.Models` | Domain Model，零相依 |
| `CakeShop.Core` | `CakeShop.Core.Interfaces` / `.DTOs` | 介面定義與資料傳輸物件 |
| `CakeShop.Infrastructure` | `CakeShop.Infrastructure` | EF Core Repository 實作 |

### 請求流程與專案依賴

```
瀏覽器
  └─▶ EC.B2C (Controller)
        └─▶ EC.CommonService (Service)
              └─▶ CakeShop.Core (Interface)
                    └─▶ CakeShop.Infrastructure (Repository)
                              └─▶ DbContext ─▶ PostgreSQL

專案依賴方向：
EC.B2C ──▶ EC.CommonService ──▶ CakeShop.Core ◀── CakeShop.Infrastructure
  │                                    │
  └────────────────────────────────────┴──▶ EC.Entities（所有層共享實體定義）
```

- **EC.B2C**：接收請求、呼叫 Service、回傳結果，僅負責輸入輸出
- **EC.CommonService**：實作商業邏輯，依賴 `CakeShop.Core` 介面
- **EC.Entities**：純 Domain Model，不依賴任何其他專案
- **CakeShop.Core**：介面與 DTO 定義層，依賴 `EC.Entities`
- **CakeShop.Infrastructure**：EF Core Repository 實作，依賴 `CakeShop.Core` 與 `EC.Entities`

---

## 環境需求

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- PostgreSQL 9.6 以上（本機或遠端皆可）
- 瀏覽器（Chrome / Edge / Firefox 最新版）

---

## 資料庫設定

### 連線字串

位於 `_B2C/EC.B2C/appsettings.json`：

```json
{
  "ConnectionStrings": {
    "Default": "Host=127.0.0.1;Port=5432;Database=TESTDB;Username=testdb0101;Password=testdb0101"
  }
}
```

修改連線字串以符合您的環境，也可透過環境變數覆蓋：

```bash
# Linux / macOS
export ConnectionStrings__Default="Host=...;Port=5432;Database=TESTDB;..."

# Windows PowerShell
$env:ConnectionStrings__Default = "Host=...;Port=5432;Database=TESTDB;..."
```

### 初始化資料庫（首次使用）

執行 `CakeShop.DbSetup` 工具，自動完成：
1. 建立 PostgreSQL 使用者 `testdb0101`（若不存在）
2. 建立資料庫 `TESTDB`
3. 建立 5 張資料表
4. 插入種子資料（10 商品、6 分類、預設帳號、公告）

```bash
cd CakeShop.DbSetup
dotnet run
```

### 資料表結構

> 欄位命名規則：DB 使用 `snake_case`，C# Model 使用 `PascalCase`，由 `EFCore.NamingConventions` 自動對應。  
> 新語系欄位（`_th` / `_ko` / `_vi` / `_ms`）以 `ALTER TABLE ADD COLUMN IF NOT EXISTS` 新增，可安全重複執行 `CakeShop.DbSetup`。

#### categories

| 欄位 | 型別 | 說明 |
|------|------|------|
| `id` | `SERIAL PK` | |
| `name` | `VARCHAR(50)` | 繁體中文名稱 |
| `name_en` | `VARCHAR(50)` | English |
| `name_ja` | `VARCHAR(50)` | 日本語 |
| `name_zh_cn` | `VARCHAR(50)` | 简体中文 |
| `name_th` | `VARCHAR(50)` | ภาษาไทย |
| `name_ko` | `VARCHAR(50)` | 한국어 |
| `name_vi` | `VARCHAR(50)` | Tiếng Việt |
| `name_ms` | `VARCHAR(50)` | Bahasa Melayu |

#### products

| 欄位 | 型別 | 說明 |
|------|------|------|
| `id` | `SERIAL PK` | |
| `name` | `VARCHAR(100)` | 繁體中文名稱 |
| `name_en` | `VARCHAR(100)` | English |
| `name_ja` | `VARCHAR(100)` | 日本語 |
| `name_zh_cn` | `VARCHAR(100)` | 简体中文 |
| `name_th` | `VARCHAR(100)` | ภาษาไทย |
| `name_ko` | `VARCHAR(100)` | 한국어 |
| `name_vi` | `VARCHAR(100)` | Tiếng Việt |
| `name_ms` | `VARCHAR(100)` | Bahasa Melayu |
| `description` | `TEXT` | 繁體中文描述 |
| `description_en` | `TEXT` | English |
| `description_ja` | `TEXT` | 日本語 |
| `description_zh_cn` | `TEXT` | 简体中文 |
| `description_th` | `TEXT` | ภาษาไทย |
| `description_ko` | `TEXT` | 한국어 |
| `description_vi` | `TEXT` | Tiếng Việt |
| `description_ms` | `TEXT` | Bahasa Melayu |
| `price` | `NUMERIC(10,2)` | 售價（NT$） |
| `image_url` | `VARCHAR(500)` | 商品圖片 URL |
| `category_id` | `INTEGER FK` | 參照 `categories.id` |
| `is_available` | `BOOLEAN` | 是否上架 |
| `created_at` | `TIMESTAMPTZ` | |

#### users

| 欄位 | 型別 | 說明 |
|------|------|------|
| `id` | `SERIAL PK` | |
| `username` | `VARCHAR(50) UNIQUE` | 帳號（唯一） |
| `password_hash` | `VARCHAR(255)` | SHA-256(password + salt) → Base64 |
| `email` | `VARCHAR(100)` | |
| `created_at` | `TIMESTAMPTZ` | |

#### cart_items

| 欄位 | 型別 | 說明 |
|------|------|------|
| `id` | `SERIAL PK` | |
| `session_id` | `VARCHAR(100)` | 對應登入帳號名稱（已登入才可操作） |
| `product_id` | `INTEGER FK` | 參照 `products.id`（CASCADE DELETE） |
| `quantity` | `INTEGER` | 數量（> 0） |
| `created_at` | `TIMESTAMPTZ` | |
| `updated_at` | `TIMESTAMPTZ` | |

#### announcements

| 欄位 | 型別 | 說明 |
|------|------|------|
| `id` | `SERIAL PK` | |
| `content` | `TEXT` | 繁體中文公告 |
| `content_en` | `TEXT` | English |
| `content_ja` | `TEXT` | 日本語 |
| `content_zh_cn` | `TEXT` | 简体中文 |
| `content_th` | `TEXT` (nullable) | ภาษาไทย（為空時退回英文） |
| `content_ko` | `TEXT` (nullable) | 한국어（為空時退回英文） |
| `content_vi` | `TEXT` (nullable) | Tiếng Việt（為空時退回英文） |
| `content_ms` | `TEXT` (nullable) | Bahasa Melayu（為空時退回英文） |
| `is_active` | `BOOLEAN` | 是否啟用 |
| `created_at` | `TIMESTAMPTZ` | |
| `updated_at` | `TIMESTAMPTZ` | |

### EF Core Migrations

專案使用 **EF Core Migrations** 管理 Schema 版本。首次在全新環境部署時：

```bash
# 1. 確認資料庫已存在（執行 DbSetup 或手動建立）
# 2. 套用所有 Pending Migrations（在 solution 根目錄執行）
dotnet ef database update \
  --project CakeShop.Infrastructure \
  --startup-project _B2C/EC.B2C
```

**新增資料表或欄位變更時：**

```bash
# 在 solution 根目錄執行
dotnet ef migrations add <MigrationName> \
  --project CakeShop.Infrastructure \
  --startup-project _B2C/EC.B2C

dotnet ef database update \
  --project CakeShop.Infrastructure \
  --startup-project _B2C/EC.B2C
```

**檢視 Migration 狀態：**

```bash
dotnet ef migrations list \
  --project CakeShop.Infrastructure \
  --startup-project _B2C/EC.B2C
```

---

## 啟動方式

### 1. 初始化資料庫（首次執行）

```bash
cd CakeShop.DbSetup
dotnet run
```

### 2. 還原套件並建置

```bash
cd ..          # 回到 solution 根目錄
dotnet restore
dotnet build
```

### 3. 啟動 API 伺服器

```bash
cd _B2C/EC.B2C
dotnet run
```

伺服器預設監聽 **http://localhost:5000**，啟動時會顯示：
```
✔ 資料庫連線成功（TESTDB）
```

### 4. 開啟網站

| 頁面 | 網址 |
|------|------|
| 首頁 | http://localhost:5000 |
| 商品頁 | http://localhost:5000/products.html |
| 聯絡我們 | http://localhost:5000/contact.html |
| Swagger UI | http://localhost:5000/swagger |

---

## 預設帳號

| 欄位 | 值 |
|------|----|
| 帳號 | `test` |
| 密碼 | `test` |

> 密碼以 `SHA-256(password + salt)` 雜湊後存於 DB；登入後返回的 Token 以 AES-256-GCM 加密。

---

## API 端點一覽

### 認證

| 方法 | 路徑 | 說明 |
|------|------|------|
| POST | `/api/auth/login` | 登入，回傳 AES-256-GCM 加密 Token |
| POST | `/api/auth/validate` | 驗證 Token 是否有效 |

### 商品

| 方法 | 路徑 | 說明 |
|------|------|------|
| GET | `/api/product` | 取得全部商品（含分類） |
| GET | `/api/product/{id}` | 取得單一商品 |
| GET | `/api/product/categories` | 取得分類列表 |
| GET | `/api/product/category/{id}` | 依分類取得商品 |

### 購物車

| 方法 | 路徑 | 說明 |
|------|------|------|
| GET | `/api/cart/{sessionId}` | 取得購物車內容 |
| POST | `/api/cart` | 新增商品至購物車 |
| PUT | `/api/cart/{sessionId}/items/{itemId}` | 更新商品數量 |
| DELETE | `/api/cart/{sessionId}/items/{itemId}` | 移除單項商品 |
| DELETE | `/api/cart/{sessionId}` | 清空購物車 |

> `sessionId` 使用登入帳號名稱（如 `test`），確保購物車資料綁定帳號。

### 聯絡 / 公告

| 方法 | 路徑 | 說明 |
|------|------|------|
| POST | `/api/contact` | 提交聯絡表單 |
| GET | `/api/announcement` | 取得啟用中的置頂公告（從 DB 讀取） |

---

## 前端功能

| 功能 | 說明 |
|------|------|
| 多語系 | 繁中 / 简中 / English / 日本語 / ภาษาไทย / 한국어 / Tiếng Việt / Bahasa Melayu，語系存入 `localStorage` |
| 多語系實作 | `i18n.js` 含 8 語系 UI 翻譯；商品名稱 / 描述 / 分類 / 公告均存於 DB 多語系欄位，`api.js` 依 locale 查表取值 |
| 登入 | Modal 彈窗，AES-GCM Token 存入 `localStorage` |
| 購物車 | 需登入；資料綁定帳號；登出自動清空 |
| 首頁輪播 | 商品倒排取 4 項，每 4 秒自動切換 |
| 置頂公告 | 從 `GET /api/announcement` 取得，支援 8 語系（新語系退回英文） |
| Loading 動畫 | 頁面切換顯示深綠 Loading Screen |
| RWD | 手機、平板、桌機全支援 |

---

## 商品資料（儲存於 PostgreSQL）

種子資料由 `CakeShop.DbSetup` 寫入，共 10 種蛋糕，每筆含 8 語系名稱與描述。

| # | 繁中 | EN | 日本語 | 泰文 | 韓文 | 越南文 | 馬來文 | 分類 | NT$ |
|---|------|----|--------|------|------|--------|--------|------|-----|
| 1 | 巧克力熔岩蛋糕 | Chocolate Lava Cake | チョコレートラバーケーキ | เค้กช็อกโกแลตลาวา | 초콜릿 용암 케이크 | Bánh Chocolate Lava | Kek Coklat Lava | 巧克力 | 280 |
| 2 | 草莓鮮奶油蛋糕 | Strawberry Fresh Cream Cake | ストロベリーショートケーキ | เค้กครีมสดสตรอเบอร์รี่ | 딸기 생크림 케이크 | Bánh Kem Dâu Tây Tươi | Kek Krim Segar Strawberi | 水果 | 350 |
| 3 | 抹茶紅豆蛋糕 | Matcha Red Bean Cake | 抹茶小豆ケーキ | เค้กชาเขียวถั่วแดง | 말차 팥 케이크 | Bánh Trà Xanh Đậu Đỏ | Kek Teh Hijau Kacang Merah | 日式 | 320 |
| 4 | 芒果慕斯蛋糕 | Mango Mousse Cake | マンゴームースケーキ | เค้กมูสมะม่วง | 망고 무스 케이크 | Bánh Mousse Xoài | Kek Mousse Mangga | 水果 | 380 |
| 5 | 藍莓起司蛋糕 | Blueberry Cheesecake | ブルーベリーチーズケーキ | ชีสเค้กบลูเบอร์รี่ | 블루베리 치즈케이크 | Bánh Phô Mai Việt Quất | Kek Keju Blueberry | 起司 | 420 |
| 6 | 黑森林蛋糕 | Black Forest Cake | シュヴァルツヴェルダー | เค้กป่าดำ | 블랙 포레스트 케이크 | Bánh Black Forest | Kek Hutan Hitam | 巧克力 | 450 |
| 7 | 提拉米蘇 | Tiramisu | ティラミス | ทีรามิสุ | 티라미수 | Tiramisu | Tiramisu | 經典 | 360 |
| 8 | 紅絲絨蛋糕 | Red Velvet Cake | レッドベルベットケーキ | เค้กกำมะหยี่แดง | 레드 벨벳 케이크 | Bánh Red Velvet | Kek Baldu Merah | 經典 | 400 |
| 9 | 檸檬磅蛋糕 | Lemon Pound Cake | レモンパウンドケーキ | เค้กปอนด์เลมอน | 레몬 파운드 케이크 | Bánh Bơ Chanh | Kek Paun Lemon | 柑橘 | 290 |
| 10 | 焦糖蘋果塔 | Caramel Apple Tart | キャラメルアップルタルト | ทาร์ตแอปเปิลคาราเมล | 캐러멜 사과 타르트 | Bánh Táo Caramel | Tart Epal Karamel | 水果 | 340 |

---

## 加密機制

```
密碼儲存：SHA-256( password + "CakeShopPasswordSalt@2024" ) → Base64 → 存入 users.password_hash
Token 格式：AES-256-GCM 加密的 "userId|username|expiresAt"
Token 結構：nonce(12 bytes) | tag(16 bytes) | ciphertext  → Base64
```

密鑰由 `MasterSecret` 字串經 SHA-256 推導（32 bytes = AES-256 key size）。  
實際部署時應將 `MasterSecret` 改為環境變數注入。

---

## 後續擴充建議

- [x] 整合 PostgreSQL，使用 EF Core Repository
- [x] 購物車資料綁定帳號
- [x] 置頂公告從 DB 讀取
- [x] 重整 Solution 架構（_B2C / Business Libraries / Framework）
- [x] 多語系擴充至 8 種（泰文、韓文、越南文、馬來文）
- [ ] 使用環境變數管理加密金鑰與連線字串
- [ ] 加入 JWT 標準認證中介層
- [ ] 前端改為 Vue 3 SPA（Vite + Vue Router）
- [ ] 加入結帳 / 訂單管理功能
- [ ] Docker 容器化部署（含 PostgreSQL）
- [ ] 加入 EF Core 完整 Migration 歷史記錄
