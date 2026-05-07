using CakeShop.Core.Interfaces;
using CakeShop.Core.Models;

namespace CakeShop.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly List<Category> _categories;
    private readonly List<Product> _products;

    public ProductRepository()
    {
        _categories = new List<Category>
        {
            new() { Id = 1, Name = "巧克力",  NameEn = "Chocolate",  NameJa = "チョコレート", NameZhCn = "巧克力"  },
            new() { Id = 2, Name = "水果",    NameEn = "Fruit",       NameJa = "フルーツ",     NameZhCn = "水果"    },
            new() { Id = 3, Name = "日式",    NameEn = "Japanese",    NameJa = "和菓子風",     NameZhCn = "日式"    },
            new() { Id = 4, Name = "起司",    NameEn = "Cheesecake",  NameJa = "チーズケーキ", NameZhCn = "奶酪"    },
            new() { Id = 5, Name = "經典",    NameEn = "Classic",     NameJa = "クラシック",   NameZhCn = "经典"    },
            new() { Id = 6, Name = "柑橘",    NameEn = "Citrus",      NameJa = "シトラス",     NameZhCn = "柑橘"    },
        };

        _products = new List<Product>
        {
            new()
            {
                Id = 1,
                Name = "巧克力熔岩蛋糕",
                NameEn = "Chocolate Lava Cake",
                NameJa = "チョコレートラバーケーキ",
                NameZhCn = "巧克力熔岩蛋糕",
                Description = "濃郁的巧克力外殼，內部流動著滑順的巧克力漿，每一口都是幸福的滋味。",
                DescriptionEn = "Rich chocolate shell with a smooth flowing center, pure bliss in every bite.",
                DescriptionJa = "濃厚なチョコレートシェルの中に、とろけるチョコレートが流れ出す幸せの一品。",
                DescriptionZhCn = "浓郁的巧克力外壳，内部流动着顺滑的巧克力浆，每一口都是幸福的滋味。",
                Price = 280,
                ImageUrl = "https://images.unsplash.com/photo-1578985545062-69928b1d9587?w=480&h=360&fit=crop&auto=format",
                CategoryId = 1, IsAvailable = true
            },
            new()
            {
                Id = 2,
                Name = "草莓鮮奶油蛋糕",
                NameEn = "Strawberry Fresh Cream Cake",
                NameJa = "ストロベリーショートケーキ",
                NameZhCn = "草莓鲜奶油蛋糕",
                Description = "精選新鮮草莓搭配輕盈鮮奶油，清爽甜美的口感令人難以忘懷。",
                DescriptionEn = "Fresh strawberries with light whipped cream, a refreshingly sweet classic.",
                DescriptionJa = "新鮮なイチゴと軽やかな生クリームが絶妙にマッチした定番ケーキ。",
                DescriptionZhCn = "精选新鲜草莓搭配轻盈鲜奶油，清爽甜美的口感令人难以忘怀。",
                Price = 350,
                ImageUrl = "https://images.unsplash.com/photo-1565958011703-44f9829ba187?w=480&h=360&fit=crop&auto=format",
                CategoryId = 2, IsAvailable = true
            },
            new()
            {
                Id = 3,
                Name = "抹茶紅豆蛋糕",
                NameEn = "Matcha Red Bean Cake",
                NameJa = "抹茶小豆ケーキ",
                NameZhCn = "抹茶红豆蛋糕",
                Description = "精選宇治抹茶與細緻紅豆泥完美融合，帶來獨特的日式風味體驗。",
                DescriptionEn = "Premium Uji matcha blended with smooth red bean paste, a unique Japanese experience.",
                DescriptionJa = "厳選された宇治抹茶と丁寧に炊いた小豆あんが調和した和の一品。",
                DescriptionZhCn = "精选宇治抹茶与细腻红豆泥完美融合，带来独特的日式风味体验。",
                Price = 320,
                ImageUrl = "https://images.unsplash.com/photo-1556679343-c7306c1976bc?w=480&h=360&fit=crop&auto=format",
                CategoryId = 3, IsAvailable = true
            },
            new()
            {
                Id = 4,
                Name = "芒果慕斯蛋糕",
                NameEn = "Mango Mousse Cake",
                NameJa = "マンゴームースケーキ",
                NameZhCn = "芒果慕斯蛋糕",
                Description = "選用台灣愛文芒果製作，綿密慕斯搭配新鮮芒果丁，夏日最清爽的享受。",
                DescriptionEn = "Made with Irwin mangoes, silky mousse with fresh mango chunks — perfect summer treat.",
                DescriptionJa = "台湾産アーウィンマンゴー使用。なめらかなムースとフレッシュマンゴーの夏の逸品。",
                DescriptionZhCn = "选用台湾爱文芒果制作，绵密慕斯搭配新鲜芒果丁，夏日最清爽的享受。",
                Price = 380,
                ImageUrl = "https://images.unsplash.com/photo-1587314168485-3236d6710814?w=480&h=360&fit=crop&auto=format",
                CategoryId = 2, IsAvailable = true
            },
            new()
            {
                Id = 5,
                Name = "藍莓起司蛋糕",
                NameEn = "Blueberry Cheesecake",
                NameJa = "ブルーベリーチーズケーキ",
                NameZhCn = "蓝莓奶酪蛋糕",
                Description = "濃郁奶油起司底層，鋪上新鮮藍莓果醬，酸甜平衡的完美滋味。",
                DescriptionEn = "Rich cream cheese base topped with fresh blueberry compote, perfectly balanced.",
                DescriptionJa = "濃厚なクリームチーズの上に新鮮なブルーベリーソースをのせた酸甘の逸品。",
                DescriptionZhCn = "浓郁奶油奶酪底层，铺上新鲜蓝莓果酱，酸甜平衡的完美滋味。",
                Price = 420,
                ImageUrl = "https://images.unsplash.com/photo-1571115177098-24ec42ed204d?w=480&h=360&fit=crop&auto=format",
                CategoryId = 4, IsAvailable = true
            },
            new()
            {
                Id = 6,
                Name = "黑森林蛋糕",
                NameEn = "Black Forest Cake",
                NameJa = "シュヴァルツヴェルダー",
                NameZhCn = "黑森林蛋糕",
                Description = "德式傳統經典，黑巧克力蛋糕搭配酸甜酒漬車厘子與鮮奶油，層次豐富迷人。",
                DescriptionEn = "German classic with dark chocolate cake, kirsch-soaked cherries and cream layers.",
                DescriptionJa = "ダークチョコレートとキルシュ漬けチェリー、生クリームが重なるドイツの定番。",
                DescriptionZhCn = "德式传统经典，黑巧克力蛋糕搭配酸甜酒渍车厘子与鲜奶油，层次丰富迷人。",
                Price = 450,
                ImageUrl = "https://images.unsplash.com/photo-1464195244916-405fa0a82545?w=480&h=360&fit=crop&auto=format",
                CategoryId = 1, IsAvailable = true
            },
            new()
            {
                Id = 7,
                Name = "提拉米蘇",
                NameEn = "Tiramisu",
                NameJa = "ティラミス",
                NameZhCn = "提拉米苏",
                Description = "義式經典甜點，手指餅乾浸泡濃縮咖啡，搭配馬斯卡邦起司，回味無窮。",
                DescriptionEn = "Italian classic with espresso-soaked ladyfingers and velvety mascarpone cream.",
                DescriptionJa = "エスプレッソに浸したサヴォイアルディとマスカルポーネクリームのイタリア名菓。",
                DescriptionZhCn = "意式经典甜点，手指饼干浸泡浓缩咖啡，搭配马斯卡彭奶酪，回味无穷。",
                Price = 360,
                ImageUrl = "https://images.unsplash.com/photo-1542124948-dc391252a940?w=480&h=360&fit=crop&auto=format",
                CategoryId = 5, IsAvailable = true
            },
            new()
            {
                Id = 8,
                Name = "紅絲絨蛋糕",
                NameEn = "Red Velvet Cake",
                NameJa = "レッドベルベットケーキ",
                NameZhCn = "红丝绒蛋糕",
                Description = "美式經典紅絲絨，絲滑奶油起司霜配上鮮豔紅色蛋糕體，視覺與味覺的雙重享受。",
                DescriptionEn = "American classic with vibrant red cake and silky cream cheese frosting.",
                DescriptionJa = "鮮やかな赤いスポンジとなめらかなクリームチーズフロスティングの美式定番。",
                DescriptionZhCn = "美式经典红丝绒，丝滑奶油奶酪霜配上鲜艳红色蛋糕体，视觉与味觉的双重享受。",
                Price = 400,
                ImageUrl = "https://images.unsplash.com/photo-1535141192574-5d4897c12636?w=480&h=360&fit=crop&auto=format",
                CategoryId = 5, IsAvailable = true
            },
            new()
            {
                Id = 9,
                Name = "檸檬磅蛋糕",
                NameEn = "Lemon Pound Cake",
                NameJa = "レモンパウンドケーキ",
                NameZhCn = "柠檬磅蛋糕",
                Description = "新鮮檸檬汁與檸檬皮充分融入蛋糕體中，清爽酸甜，搭配糖霜更添風味。",
                DescriptionEn = "Fresh lemon juice and zest infused throughout, light and tangy with a sugar glaze.",
                DescriptionJa = "新鮮なレモン果汁と皮をたっぷり使った爽やかな酸甘のパウンドケーキ。",
                DescriptionZhCn = "新鲜柠檬汁与柠檬皮充分融入蛋糕体中，清爽酸甜，搭配糖霜更添风味。",
                Price = 290,
                ImageUrl = "https://images.unsplash.com/photo-1519915028421-c6c1cb6c7a8b?w=480&h=360&fit=crop&auto=format",
                CategoryId = 6, IsAvailable = true
            },
            new()
            {
                Id = 10,
                Name = "焦糖蘋果塔",
                NameEn = "Caramel Apple Tart",
                NameJa = "キャラメルアップルタルト",
                NameZhCn = "焦糖苹果塔",
                Description = "酥脆塔皮搭配焦糖蘋果，淋上香濃焦糖醬，法式風情盡在其中。",
                DescriptionEn = "Crispy tart shell with caramelized apples and a rich caramel drizzle, French elegance.",
                DescriptionJa = "サクサクのタルト生地にキャラメルアップルを並べ、濃厚なキャラメルソースを添えた逸品。",
                DescriptionZhCn = "酥脆塔皮搭配焦糖苹果，淋上香浓焦糖酱，法式风情尽在其中。",
                Price = 340,
                ImageUrl = "https://images.unsplash.com/photo-1562440499-64c9a111f713?w=480&h=360&fit=crop&auto=format",
                CategoryId = 2, IsAvailable = true
            }
        };

        foreach (var p in _products)
            p.Category = _categories.FirstOrDefault(c => c.Id == p.CategoryId);
    }

    public Task<IEnumerable<Product>> GetAllAsync()
        => Task.FromResult(_products.AsEnumerable());

    public Task<Product?> GetByIdAsync(int id)
        => Task.FromResult(_products.FirstOrDefault(p => p.Id == id));

    public Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        => Task.FromResult(_products.Where(p => p.CategoryId == categoryId));

    public Task<IEnumerable<Category>> GetCategoriesAsync()
        => Task.FromResult(_categories.AsEnumerable());
}
