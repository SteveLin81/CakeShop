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
| 後端框架 | ASP.NET Core 8 Web API + Razor Pages |
| 語言 | C# 12 |
| ORM | Entity Framework Core 8（Npgsql 驅動） |
| 資料庫 | PostgreSQL 9.6+ |
| 前端框架 | Vue 3（CDN）、Element Plus、Vue-i18n |
| 頁面引擎 | ASP.NET Core Razor Pages（`_Layout.cshtml` 共用結構） |
| 加密 | SHA-256 密碼雜湊 + AES-256-GCM Token |
| API 文件 | Swagger / Swashbuckle |

---

## 專案架構

Solution 依用途分為六個 Solution Folder：

```
CakeShop/
│
├── 📁 _B2C/                                    ← Solution Folder：B2C 消費者網站
│   └── EC.B2C/                                 # 網站入口層（Razor Pages + Web API）
│       ├── Pages/                              # Razor Pages
│       │   ├── Shared/
│       │   │   └── _Layout.cshtml              # 共用 Layout（Navbar、公告Bar、Footer）
│       │   ├── _ViewImports.cshtml
│       │   ├── _ViewStart.cshtml
│       │   ├── Index.cshtml / .cs              # 首頁（Hero、輪播、精選商品）
│       │   ├── Products.cshtml / .cs           # 商品頁（篩選、搜尋）
│       │   └── Contact.cshtml / .cs            # 聯絡頁
│       ├── Controllers/
│       │   ├── AuthController.cs
│       │   ├── ProductController.cs
│       │   ├── CartController.cs
│       │   ├── ContactController.cs
│       │   └── AnnouncementController.cs
│       ├── Program.cs                          # DI 註冊、Razor Pages、DbContext、Swagger
│       ├── appsettings.json                    # Port 5000、ConnectionStrings
│       └── wwwroot/                            # 靜態資源
│           ├── css/style.css
│           └── js/
│               ├── i18n.js                     # 8 語系翻譯字典
│               ├── api.js                      # API 呼叫封裝
│               └── common.js                   # 共用 Vue Composition（語言、驗證、購物車、公告）
│
├── 📁 _B2E/                                    ← Solution Folder：B2E 後台管理系統
│   └── EC.B2E/                                 # 後台入口層（Razor Pages + Web API）
│       ├── Pages/                              # Razor Pages（需 B2E Token）
│       │   ├── Admin/
│       │   │   ├── _AdminLayout.cshtml         # 後台共用 Layout（側邊欄、頂欄、語言切換）
│       │   │   ├── Dashboard.cshtml / .cs      # 儀表板（統計數字、快速連結）
│       │   │   ├── Products.cshtml / .cs       # 商品管理（8語系 CRUD + 圖片上傳 + 精選開關）
│       │   │   ├── Categories.cshtml / .cs     # 分類管理（8語系 CRUD）
│       │   │   ├── Announcements.cshtml / .cs  # 公告管理（啟用切換 + CRUD）
│       │   │   ├── Users.cshtml / .cs          # B2C 帳號管理（CRUD + 即時搜尋）
│       │   │   └── Homepage.cshtml / .cs       # 首頁設定（精選/輪播商品 switch 管理）
│       │   ├── Login.cshtml / .cs              # 登入頁（獨立 Layout）
│       │   ├── _ViewImports.cshtml
│       │   └── _ViewStart.cshtml
│       ├── Controllers/
│       │   ├── B2eAuthController.cs            # POST login / validate（Rate Limiting）
│       │   ├── B2eProductController.cs         # 商品 CRUD + POST upload-image
│       │   ├── B2eCategoryController.cs        # 分類 CRUD（8語系）
│       │   ├── B2eAnnouncementController.cs    # 公告 CRUD + PATCH activate
│       │   └── B2eUserController.cs            # B2C 帳號 CRUD
│       ├── Filters/
│       │   └── B2eAuthFilter.cs                # IAsyncActionFilter：Bearer Token 驗證
│       ├── Program.cs                          # DI 註冊、B2E 服務、Swagger（Bearer auth）
│       ├── appsettings.json                    # Port 5200、ConnectionStrings（同 B2C DB）
│       └── wwwroot/                            # 靜態資源
│           ├── css/b2e.css                     # 後台樣式（深藍灰側邊欄 + 藍色 Accent）
│           └── js/
│               ├── b2e-i18n.js                 # 8 語系後台翻譯字典
│               ├── b2e-api.js                  # 封裝 /api/b2e/* 呼叫（自動帶 Token）
│               └── b2e-common.js               # useB2eCommon()：Auth Guard / Toast / 語言切換
│
├── 📁 _API/                                    ← Solution Folder：對外查詢 API
│   └── EC.API/                                 # 外部 RESTful 查詢 API
│       ├── Controllers/
│       │   ├── ProductsController.cs           # 商品與分類查詢
│       │   ├── AnnouncementsController.cs      # 公告查詢
│       │   └── MembersController.cs            # 會員資訊查詢
│       ├── Program.cs                          # DI 註冊、DbContext、Swagger（根路徑）
│       └── appsettings.json                    # Port 5100、ConnectionStrings
│
├── 📁 _Test/                                   ← Solution Folder：單元測試
│   └── EC.Test/                                # xUnit + Moq + FluentAssertions
│       ├── GlobalUsings.cs                     # global using Xunit / Moq / FluentAssertions
│       └── Services/
│           ├── EncryptionServiceTests.cs       # 13 個測試
│           ├── AuthServiceTests.cs             # 10 個測試
│           ├── ProductServiceTests.cs          # 11 個測試
│           ├── CartServiceTests.cs             # 13 個測試
│           ├── AnnouncementServiceTests.cs     #  7 個測試
│           └── ContactServiceTests.cs          #  3 個測試
│
├── 📁 Business Libraries/                      ← Solution Folder：商業邏輯
│   └── EC.CommonService/
│       └── Services/
│           ├── EncryptionService.cs            # SHA-256 雜湊 + AES-256-GCM 加解密（密鑰由 IConfiguration 注入）
│           ├── AuthService.cs                  # B2C 登入驗證、Token 產生
│           ├── B2eAuthService.cs               # B2E 管理員登入驗證、Token 產生
│           ├── ProductService.cs               # 商品查詢邏輯（含 IsFeatured）
│           ├── ProductManagementService.cs     # 商品 CRUD（供 B2E 後台，含 IsFeatured）
│           ├── CategoryManagementService.cs    # 分類 CRUD（供 B2E 後台）
│           ├── CartService.cs                  # 購物車操作邏輯
│           ├── ContactService.cs               # 聯絡表單處理
│           ├── AnnouncementService.cs          # 置頂公告查詢（B2C 使用）
│           ├── AnnouncementManagementService.cs# 公告 CRUD + 啟用切換（供 B2E 後台）
│           └── B2cUserManagementService.cs     # B2C 帳號 CRUD（供 B2E 後台）
│
├── 📁 Framework/                               ← Solution Folder：基礎架構
│   └── EC.Entities/                            # Domain 實體層（無任何外部相依）
│       └── Models/
│           ├── Product.cs
│           ├── Category.cs
│           ├── User.cs
│           ├── B2eUser.cs                      # B2E 管理員帳號實體（獨立資料表）
│           ├── CartItem.cs
│           └── Announcement.cs
│
├── CakeShop.Core/                              # 合約層（DTOs、Interfaces）
│   ├── DTOs/
│   │   ├── ProductDto.cs / CategoryDto.cs
│   │   ├── CartDto.cs
│   │   ├── LoginRequest.cs / LoginResponse.cs
│   │   ├── ContactFormDto.cs
│   │   ├── AnnouncementDto.cs
│   │   └── B2eDto.cs                          # B2E 專用 DTO（ApiResult<T> / B2cUserDto / CRUD Request）
│   └── Interfaces/
│       ├── IEncryptionService.cs
│       ├── IAuthService.cs / IProductService.cs / ICartService.cs
│       ├── IContactService.cs / IAnnouncementService.cs
│       ├── IB2eAuthService.cs                 # B2E 認證介面
│       ├── IB2eUserRepository.cs              # B2E 管理員 Repository 介面
│       ├── IProductManagementService.cs       # 商品 CRUD 介面（B2E 用）
│       ├── ICategoryManagementService.cs      # 分類 CRUD 介面（B2E 用）
│       ├── IAnnouncementManagementService.cs  # 公告 CRUD 介面（B2E 用）
│       ├── IB2cUserManagementService.cs       # B2C 帳號 CRUD 介面（B2E 用）
│       ├── IProductRepository.cs              # 含分類 CRUD 方法（GetCategoryById / Create / Update / Delete）
│       ├── IUserRepository.cs
│       └── ICartRepository.cs / IAnnouncementRepository.cs
│
├── CakeShop.Infrastructure/                    # 資料存取層（EF Core）
│   ├── Data/
│   │   ├── CakeShopDbContext.cs                # EF Core DbContext（含 B2eUsers DbSet）
│   │   └── DesignTimeDbContextFactory.cs       # 供 dotnet ef 使用
│   └── Repositories/
│       ├── ProductRepository.cs               # 含 B2E CRUD 方法擴充
│       ├── UserRepository.cs                  # 含 B2E CRUD 方法擴充
│       ├── CartRepository.cs
│       ├── AnnouncementRepository.cs          # 含 B2E CRUD 方法擴充
│       └── B2eUserRepository.cs               # B2E 管理員帳號查詢
│
└── CakeShop.DbSetup/                           # 資料庫初始化工具
    └── Program.cs                              # 建立 DB、6 張資料表、種子資料（含 b2e_users）
```

### 命名空間對應

| 專案 | 命名空間 | 職責 |
|------|---------|------|
| `EC.B2C` | `EC.B2C.Pages` / `EC.B2C.Controllers` | B2C 消費者網站：Razor Pages + Web API |
| `EC.B2E` | `EC.B2E.Pages` / `EC.B2E.Controllers` | B2E 後台管理：Razor Pages + 管理 API |
| `EC.API` | `EC.API.Controllers` | 對外查詢 RESTful API |
| `EC.Test` | `EC.Test.Services` | 單元測試（xUnit + Moq） |
| `EC.CommonService` | `EC.CommonService.Services` | 商業邏輯實作 |
| `EC.Entities` | `EC.Entities.Models` | Domain Model，零相依 |
| `CakeShop.Core` | `CakeShop.Core.Interfaces` / `.DTOs` | 介面定義與資料傳輸物件 |
| `CakeShop.Infrastructure` | `CakeShop.Infrastructure` | EF Core Repository 實作 |

### 請求流程與專案依賴

```
瀏覽器（消費者）      管理員瀏覽器       外部呼叫端
      │                   │                 │
      ▼                   ▼                 ▼
EC.B2C (Port 5000)  EC.B2E (Port 5200)  EC.API (Port 5100)
      │                   │                 │
      └──────────┬─────────┘                │
                 └──────────────────────────┘
                           │
                   EC.CommonService (Service)
                           │
                   CakeShop.Core (Interface)
                           │
                 CakeShop.Infrastructure (Repository)
                           │
                    DbContext ─▶ PostgreSQL

專案依賴方向：
EC.B2C / EC.B2E / EC.API ──▶ EC.CommonService ──▶ CakeShop.Core ◀── CakeShop.Infrastructure
       │                                                 │
       └─────────────────────────────────────────────────┴──▶ EC.Entities（所有層共享實體定義）
```

- **EC.B2C**：消費者網站，Razor Pages + Vue 3；登入 / 購物車 / 商品瀏覽
- **EC.B2E**：管理員後台，深藍灰側邊欄；商品 CRUD / 公告管理 / B2C 帳號管理
- **EC.API**：對外唯讀查詢 API，Swagger UI 預設顯示於根路徑
- **EC.Test**：單元測試，使用 Moq 隔離外部相依
- **EC.CommonService**：B2C / B2E 共享商業邏輯，依賴 `CakeShop.Core` 介面
- **EC.Entities**：純 Domain Model，不依賴任何其他專案
- **CakeShop.Core**：介面與 DTO 定義層，依賴 `EC.Entities`
- **CakeShop.Infrastructure**：EF Core Repository 實作，B2E CRUD 方法在此擴充

---

## 環境需求

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- PostgreSQL 9.6 以上（本機或遠端皆可）
- 瀏覽器（Chrome / Edge / Firefox 最新版）

---

## 資料庫設定

### 連線字串

EC.B2C、EC.B2E、EC.API 共用同一個 PostgreSQL 資料庫，連線字串位於各專案的 `appsettings.json`：

```json
{
  "ConnectionStrings": {
    "Default": "Host=127.0.0.1;Port=5432;Database=TESTDB;Username=testdb0101;Password=testdb0101"
  },
  "Encryption": {
    "MasterSecret": "CHANGE_ME_IN_PRODUCTION_USE_ENV_VAR",
    "PasswordSalt": "CHANGE_ME_IN_PRODUCTION_USE_ENV_VAR"
  },
  "Cors": {
    "AllowedOrigins": []
  },
  "EnableSwagger": false
}
```

開發環境的實際密鑰放在 `appsettings.Development.json`（已加入 `.gitignore` 原則，請勿提交正式密鑰）。

修改連線字串以符合您的環境，也可透過環境變數覆蓋：

```bash
# Linux / macOS
export ConnectionStrings__Default="Host=...;Port=5432;Database=TESTDB;..."
export Encryption__MasterSecret="your-secret"
export Encryption__PasswordSalt="your-salt"

# Windows PowerShell
$env:ConnectionStrings__Default = "Host=...;Port=5432;Database=TESTDB;..."
$env:Encryption__MasterSecret = "your-secret"
$env:Encryption__PasswordSalt = "your-salt"
```

### 初始化資料庫（首次使用）

執行 `CakeShop.DbSetup` 工具，自動完成：
1. 建立 PostgreSQL 使用者 `testdb0101`（若不存在）
2. 建立資料庫 `TESTDB`
3. 建立 **6 張資料表**（含 `b2e_users`）
4. 插入種子資料（10 商品、6 分類、B2C 預設帳號、B2E 管理員帳號、公告）

```bash
cd CakeShop.DbSetup
dotnet run
```

### 資料表結構

> 欄位命名規則：DB 使用 `snake_case`，C# Model 使用 `PascalCase`，由 `EFCore.NamingConventions` 自動對應。  
> 所有資料表皆繼承 `AuditableEntity`，包含 5 個共用稽核欄位（見下方）。

#### 共用稽核欄位（所有資料表）

| 欄位 | 型別 | 說明 |
|------|------|------|
| `created_at` | `TIMESTAMPTZ NOT NULL DEFAULT NOW()` | 建立時間，由 DbContext 自動填寫 |
| `created_by` | `VARCHAR(100) NOT NULL DEFAULT 'admin'` | 建立者，B2E 操作時記錄管理員帳號 |
| `updated_at` | `TIMESTAMPTZ NOT NULL DEFAULT NOW()` | 更新時間，每次 SaveChanges 自動更新 |
| `updated_by` | `VARCHAR(100) NOT NULL DEFAULT 'admin'` | 更新者，B2E 操作時記錄管理員帳號 |
| `update_count` | `INTEGER NOT NULL DEFAULT 0` | 更新次數，每次 Modified 自動遞增 |

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
| `name_en` … `name_ms` | `VARCHAR(100)` | 其餘 7 語系名稱 |
| `description` | `TEXT` | 繁體中文描述 |
| `description_en` … `description_ms` | `TEXT` | 其餘 7 語系描述 |
| `price` | `NUMERIC(10,2)` | 售價（NT$） |
| `image_url` | `VARCHAR(500)` | 商品圖片 URL |
| `category_id` | `INTEGER FK` | 參照 `categories.id` |
| `is_available` | `BOOLEAN` | 是否上架（B2E 可切換） |
| `is_featured`  | `BOOLEAN DEFAULT FALSE` | 是否為精選/輪播商品（B2E 首頁設定可切換） |

#### users（B2C 消費者帳號）

| 欄位 | 型別 | 說明 |
|------|------|------|
| `id` | `SERIAL PK` | |
| `username` | `VARCHAR(50) UNIQUE` | 帳號（唯一） |
| `password_hash` | `VARCHAR(255)` | SHA-256(password + salt) → Base64 |
| `email` | `VARCHAR(100)` | |

#### b2e_users（B2E 管理員帳號）

| 欄位 | 型別 | 說明 |
|------|------|------|
| `id` | `SERIAL PK` | |
| `username` | `VARCHAR(50) UNIQUE` | 管理員帳號（唯一，與 B2C users 完全獨立） |
| `password_hash` | `VARCHAR(255)` | 同 B2C 雜湊格式 |
| `email` | `VARCHAR(100)` | |

> `b2e_users` 與 `users` 為**獨立資料表**，帳號體系完全分離；B2E Token 也獨立於 B2C Token 之外。

#### cart_items

| 欄位 | 型別 | 說明 |
|------|------|------|
| `id` | `SERIAL PK` | |
| `session_id` | `VARCHAR(100)` | 對應登入帳號名稱（已登入才可操作） |
| `product_id` | `INTEGER FK` | 參照 `products.id`（CASCADE DELETE） |
| `quantity` | `INTEGER` | 數量（> 0） |

#### announcements

| 欄位 | 型別 | 說明 |
|------|------|------|
| `id` | `SERIAL PK` | |
| `content` | `TEXT` | 繁體中文公告 |
| `content_en` … `content_ms` | `TEXT` (nullable) | 其餘 7 語系（為空時退回英文） |
| `is_active` | `BOOLEAN` | 是否啟用（同時只有 1 筆啟用，B2E 可切換） |

### EF Core Migrations

```bash
# 套用所有 Pending Migrations（在 solution 根目錄執行）
dotnet ef database update \
  --project CakeShop.Infrastructure \
  --startup-project _B2C/EC.B2C

# 新增 Migration
dotnet ef migrations add <MigrationName> \
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

### 3. 啟動 B2C 消費者網站

```bash
cd _B2C/EC.B2C
dotnet run
```

伺服器監聽 **http://localhost:5000**

### 4. 啟動 B2E 後台管理系統

```bash
cd _B2E/EC.B2E
dotnet run
```

伺服器監聽 **http://localhost:5200**

### 5. 啟動對外 API（選用）

```bash
cd _API/EC.API
dotnet run
```

伺服器監聽 **http://localhost:5100**，根路徑直接顯示 Swagger UI。

### 6. 開啟網站

> **HTTPS 說明**：開發環境（`ASPNETCORE_ENVIRONMENT=Development`）使用 **HTTP**；正式環境自動強制重導至 HTTPS。  
> 若要在本機測試 HTTPS，請先執行 `dotnet dev-certs https --trust` 安裝開發用 SSL 憑證。

| 頁面 | 開發環境網址 |
|------|------------|
| B2C 首頁 | http://localhost:5000 |
| B2C 商品頁 | http://localhost:5000/Products |
| B2C 聯絡我們 | http://localhost:5000/Contact |
| B2C Swagger UI | http://localhost:5000/swagger |
| **B2E 登入頁** | **http://localhost:5200/b2e/login** |
| **B2E 儀表板** | **http://localhost:5200/b2e/admin** |
| **B2E Swagger UI** | **http://localhost:5200/swagger** |
| EC.API Swagger UI | http://localhost:5100 |

### 7. 執行單元測試

```bash
cd _Test/EC.Test
dotnet test
```

預期結果：
```
已通過! - 失敗: 0，通過: 61，略過: 0，總計: 61
```

---

## 預設帳號

### B2C 消費者帳號

| 欄位 | 值 |
|------|----|
| 帳號 | `test` |
| 密碼 | `test` |

### B2E 管理員帳號

| 欄位 | 值 |
|------|----|
| 帳號 | `testb2e` |
| 密碼 | `testb2e` |
| 登入頁 | http://localhost:5200/b2e/login |

> 兩組帳號使用相同的 `SHA-256(password + salt)` 雜湊機制，但儲存於完全獨立的資料表；Token 加密格式相同（AES-256-GCM），但驗證邏輯獨立，彼此無法互用。

---

## API 端點一覽

### EC.B2C（Port 5000）— 消費者 API

#### 認證

| 方法 | 路徑 | 說明 |
|------|------|------|
| POST | `/api/auth/login` | B2C 登入，回傳 AES-256-GCM Token |
| POST | `/api/auth/validate` | 驗證 B2C Token |

#### 商品

| 方法 | 路徑 | 說明 |
|------|------|------|
| GET | `/api/product` | 取得全部商品（含分類） |
| GET | `/api/product/{id}` | 取得單一商品 |
| GET | `/api/product/categories` | 取得分類列表 |
| GET | `/api/product/category/{id}` | 依分類取得商品 |

#### 購物車

| 方法 | 路徑 | 說明 |
|------|------|------|
| GET | `/api/cart/{sessionId}` | 取得購物車內容 |
| POST | `/api/cart` | 新增商品至購物車 |
| PUT | `/api/cart/{sessionId}/items/{itemId}` | 更新商品數量 |
| DELETE | `/api/cart/{sessionId}/items/{itemId}` | 移除單項商品 |
| DELETE | `/api/cart/{sessionId}` | 清空購物車 |

#### 聯絡 / 公告

| 方法 | 路徑 | 說明 |
|------|------|------|
| POST | `/api/contact` | 提交聯絡表單 |
| GET | `/api/announcement` | 取得啟用中的置頂公告（從 DB 讀取） |

---

### EC.B2E（Port 5200）— 後台管理 API

所有 `/api/b2e/*` 端點（auth 除外）均需在 `Authorization` Header 帶入 B2E Token：
```
Authorization: Bearer <b2eToken>
```

#### 認證

| 方法 | 路徑 | 說明 |
|------|------|------|
| POST | `/api/b2e/auth/login` | B2E 管理員登入，回傳 Token |
| POST | `/api/b2e/auth/validate` | 驗證 B2E Token 是否有效 |

#### 商品管理

| 方法 | 路徑 | 說明 |
|------|------|------|
| GET | `/api/b2e/products` | 取得全部商品（含 `isFeatured` 欄位） |
| GET | `/api/b2e/products/{id}` | 取得單一商品 |
| GET | `/api/b2e/products/categories` | 取得分類列表（唯讀快捷） |
| POST | `/api/b2e/products` | 新增商品（8 語系 + `isFeatured`） |
| PUT | `/api/b2e/products/{id}` | 修改商品（含 `isFeatured` 切換） |
| DELETE | `/api/b2e/products/{id}` | 刪除商品 |
| POST | `/api/b2e/products/upload-image` | 上傳商品圖片（≤5 MB，jpg/png/webp/gif），回傳 `/uploads/products/{uuid}.ext` |

#### 分類管理

| 方法 | 路徑 | 說明 |
|------|------|------|
| GET | `/api/b2e/categories` | 取得全部分類（含 8 語系） |
| GET | `/api/b2e/categories/{id}` | 取得單一分類 |
| POST | `/api/b2e/categories` | 新增分類（8 語系） |
| PUT | `/api/b2e/categories/{id}` | 修改分類 |
| DELETE | `/api/b2e/categories/{id}` | 刪除分類（有商品使用時回傳錯誤） |

#### 公告管理

| 方法 | 路徑 | 說明 |
|------|------|------|
| GET | `/api/b2e/announcements` | 取得所有公告 |
| GET | `/api/b2e/announcements/{id}` | 取得單一公告 |
| POST | `/api/b2e/announcements` | 新增公告（8 語系） |
| PUT | `/api/b2e/announcements/{id}` | 修改公告 |
| DELETE | `/api/b2e/announcements/{id}` | 刪除公告 |
| PATCH | `/api/b2e/announcements/{id}/activate` | 啟用指定公告（其餘自動停用） |

#### B2C 帳號管理

| 方法 | 路徑 | 說明 |
|------|------|------|
| GET | `/api/b2e/users` | 取得所有 B2C 帳號 |
| GET | `/api/b2e/users/{id}` | 取得單一 B2C 帳號 |
| POST | `/api/b2e/users` | 新增 B2C 帳號 |
| PUT | `/api/b2e/users/{id}` | 修改 B2C 帳號（Email / 密碼） |
| DELETE | `/api/b2e/users/{id}` | 刪除 B2C 帳號 |

---

### EC.API（Port 5100）— 對外查詢 API

Swagger UI 位於根路徑：**http://localhost:5100**

#### 商品

| 方法 | 路徑 | 說明 |
|------|------|------|
| GET | `/api/products` | 取得全部商品（含 8 語系） |
| GET | `/api/products/{id}` | 依 ID 查詢商品 |
| GET | `/api/products/category/{categoryId}` | 依分類查詢商品 |
| GET | `/api/products/categories` | 取得所有分類 |

#### 公告

| 方法 | 路徑 | 說明 |
|------|------|------|
| GET | `/api/announcements/active` | 取得目前生效中的公告 |

#### 會員

| 方法 | 路徑 | 說明 |
|------|------|------|
| GET | `/api/members/{username}` | 依帳號查詢 B2C 會員（不含 PasswordHash） |
| GET | `/api/members/id/{id}` | 依 ID 查詢 B2C 會員 |

---

## 前端架構

### B2C：Razor Pages + Vue 3 混合模式

```
瀏覽器請求
  │
  ▼
Razor Pages（Server-side）
  ├── _Layout.cshtml  ─── 共用結構：Navbar、公告Bar、Footer、購物車抽屜、登入 Modal
  ├── Index.cshtml    ─── 首頁模板
  ├── Products.cshtml ─── 商品頁模板
  └── Contact.cshtml  ─── 聯絡頁模板
          │
          ▼（嵌入 Vue 3 Client-side）
      Vue createApp 掛載 #app
          ├── common.js  ─── useCommonSetup()：語言 / 驗證 / 購物車 / 公告
          ├── api.js     ─── 封裝所有後端 API 呼叫
          └── i18n.js    ─── 8 語系翻譯字典
```

### B2E：Razor Pages + Vue 3 + Element Plus 後台

```
瀏覽器請求（需 B2E Token）
  │
  ▼
Razor Pages（Server-side）
  ├── Login.cshtml         ─── 獨立登入頁（Rate Limiting：1分鐘5次）
  └── Admin/_AdminLayout.cshtml ─── 後台共用 Layout
      ├── 深藍灰側邊欄（Dashboard / Products / Categories / Announcements / Users / Homepage）
      ├── 頂欄（語言切換 / 管理員帳號 / 登出）
      └── @RenderBody()
              │
              ▼（@section Scripts 嵌入 Vue 3 + Element Plus）
          各管理頁 createApp
              ├── b2e-common.js  ─── useB2eCommon()：Auth Guard / Toast / 語言切換
              ├── b2e-api.js     ─── 封裝 /api/b2e/* 呼叫（自動帶 b2eToken，含圖片上傳）
              └── b2e-i18n.js    ─── 8 語系後台翻譯字典
```

**B2E 後台頁面**

| 路徑 | 功能 |
|------|------|
| `/b2e/login` | 管理員登入，`testb2e / testb2e` |
| `/b2e/admin` | 儀表板：統計數字 + 六個快捷卡 |
| `/b2e/admin/products` | 商品管理：8語系 CRUD、圖片上傳、⭐精選開關 |
| `/b2e/admin/categories` | 分類管理：8語系 CRUD，刪除前須確保無商品使用 |
| `/b2e/admin/announcements` | 公告管理：8語系 CRUD + 一鍵啟用（其餘自動停用） |
| `/b2e/admin/users` | B2C 帳號管理：新增/編輯/刪除、帳號或 Email 即時搜尋 |
| `/b2e/admin/homepage` | 首頁設定：所有商品列表 + Switch 切換精選狀態，精選商品自動出現在 B2C 輪播 |

**B2E 前端功能**

| 功能 | 說明 |
|------|------|
| 認證 Guard | `useB2eCommon().checkAuth()` 在每頁 `onMounted` 驗證 Token，失效即跳轉 `/b2e/login` |
| 商品管理 | `el-table`、`el-dialog` + `el-tabs`（8語言分頁）、圖片上傳按鈕、精選⭐欄位 |
| 分類管理 | 完整 8語系 CRUD，分類名稱按語言 Tab 填寫 |
| 公告管理 | CRUD + 啟用切換（`PATCH /activate`）；啟用後其他公告自動停用 |
| 首頁設定 | 商品列表 + `el-switch` 即時切換 `isFeatured`，B2C 輪播優先顯示精選商品 |
| B2C 帳號管理 | 帳號 / Email 即時搜尋，支援新增 / 修改密碼 / 刪除 |
| 圖片上傳 | 商品表單內建上傳按鈕，圖片存於 `wwwroot/uploads/products/`，限 5 MB |
| 多語系 | 8 種語言切換（`b2e-i18n.js`），語系存入 `localStorage['b2eLocale']` |
| Token 儲存 | `localStorage['b2eToken']`、`localStorage['b2eUsername']`（與 B2C Token 獨立） |

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
密碼儲存：SHA-256( password + PasswordSalt ) → Base64 → 存入 users.password_hash / b2e_users.password_hash
Token 格式：AES-256-GCM 加密的 "userId|username|expiresAt"
Token 結構：nonce(12 bytes) | tag(16 bytes) | ciphertext  → Base64
```

B2C 與 B2E 使用相同的加密格式，但 Token 分別由各自的 `AuthService` / `B2eAuthService` 產生與驗證，不可互用。

### 密鑰設定

加密密鑰由 `appsettings.json` 的 `Encryption` 區段讀取，`EncryptionService` 在建構時透過 `IConfiguration` 注入：

```json
{
  "Encryption": {
    "MasterSecret": "CakeShopMasterSecret2024!#$%AES256GCM",
    "PasswordSalt": "CakeShopPasswordSalt@2024"
  }
}
```

> **⚠️ 注意（登入 401 常見原因）**  
> 若以 `Start-Process` 或背景程序啟動服務，且未設定 `ASPNETCORE_ENVIRONMENT=Development`，  
> 服務將只讀取 `appsettings.json`（不讀 `appsettings.Development.json`）。  
> 此時 `Encryption.PasswordSalt` 若與 DB 中的密碼雜湊使用的 salt 不同，登入會回傳 **401 Unauthorized**。  
> **解法**：確保 `appsettings.json` 的 Encryption 值與 DbSetup 使用的 salt 相同，或明確設定環境變數。

生產環境部署時，以環境變數覆蓋密鑰（優先於設定檔）：

```bash
# Linux / macOS
export Encryption__MasterSecret="your-production-secret"
export Encryption__PasswordSalt="your-production-salt"

# Windows PowerShell
$env:Encryption__MasterSecret = "your-production-secret"
$env:Encryption__PasswordSalt = "your-production-salt"
```

---

## 資安機制

### HTTPS 強制重導

正式環境（非 Development）自動啟用 `UseHttpsRedirection()`，所有 HTTP 請求重導至 HTTPS。  
開發環境保持 HTTP 以避免缺少 SSL 憑證時無法啟動；本機如需測試 HTTPS 請執行：

```bash
dotnet dev-certs https --trust
```

### HTTP 安全標頭

所有回應自動附加以下標頭：

| 標頭 | 值 | 用途 |
|------|-----|------|
| `X-Content-Type-Options` | `nosniff` | 防止 MIME-type sniffing |
| `X-Frame-Options` | `SAMEORIGIN` | 防止 Clickjacking |
| `X-XSS-Protection` | `1; mode=block` | 舊式瀏覽器 XSS 過濾 |
| `Referrer-Policy` | `strict-origin-when-cross-origin` | 控制 Referer 標頭洩漏範圍 |
| `Permissions-Policy` | `geolocation=(), microphone=(), camera=()` | 關閉非必要瀏覽器 API |

### Rate Limiting（速率限制）

登入端點（`/api/auth/login`、`/api/b2e/auth/login`）每 IP **每分鐘最多 5 次**，超過回傳 `429 Too Many Requests`。  
使用 ASP.NET Core 8 內建 `AddRateLimiter` Fixed Window 策略。

### CORS 設定

CORS 來源由設定檔控制：

```json
{
  "Cors": {
    "AllowedOrigins": ["https://your-domain.com"]
  }
}
```

`AllowedOrigins` 為空陣列時允許所有來源（開發便利用），生產環境**必須**設定實際網域。

### Swagger UI 存取控制

Swagger UI 僅在開發環境或明確設定 `"EnableSwagger": true` 時開啟：

```json
{ "EnableSwagger": true }   // appsettings.Development.json（已預設開啟）
{ "EnableSwagger": false }  // appsettings.json 正式環境（預設關閉）
```

### API 輸入驗證

所有 B2E 管理 API 的 Request DTO 均加入 Data Annotations：

| DTO | 驗證規則 |
|-----|---------|
| `B2cUserCreateRequest` | Username: `[Required][MaxLength(50)][RegularExpression 英數字]`；Password: `[Required][MinLength(6)]`；Email: `[EmailAddress]` |
| `B2cUserUpdateRequest` | Email: `[EmailAddress]`；NewPassword: `[MinLength(6)]`（可選） |
| `ProductSaveRequest` | Price: `[Range(0, 999999)]`；CategoryId: `[Range(1, int.MaxValue)]`；Name: `[MaxLength(100)]` |

---

## 單元測試

使用 **xUnit + Moq + FluentAssertions**，共 61 個測試，全數通過。

| 測試類別 | 測試數 | 涵蓋範圍 |
|---------|--------|---------|
| `EncryptionServiceTests` | 13 | HashPassword / VerifyPassword / AES-GCM 加解密 / 竄改偵測 |
| `AuthServiceTests` | 10 | LoginAsync / ValidateTokenAsync / GetUsernameFromTokenAsync |
| `ProductServiceTests` | 11 | 商品查詢、分類查詢、nullable 語系欄位 fallback、DTO 對應 |
| `CartServiceTests` | 13 | GetCart / AddToCart / UpdateQuantity / Remove / Clear |
| `AnnouncementServiceTests` | 7 | 公告查詢、4 種語系 null fallback |
| `ContactServiceTests` | 3 | 表單提交成功回應 |

```bash
# 執行測試
dotnet test _Test/EC.Test

# 已通過! - 失敗: 0，通過: 61，略過: 0，總計: 61
```

---

## 後續擴充建議

- [x] 整合 PostgreSQL，使用 EF Core Repository
- [x] 購物車資料綁定帳號
- [x] 置頂公告從 DB 讀取
- [x] 重整 Solution 架構（_B2C / _API / _Test / Business Libraries / Framework）
- [x] 多語系擴充至 8 種（泰文、韓文、越南文、馬來文）
- [x] 新增 EC.API 對外查詢 API（商品、公告、會員，Port 5100）
- [x] 建立 EC.Test 單元測試專案（61 個測試全數通過）
- [x] 靜態 HTML 改為 Razor Pages（`_Layout.cshtml` 共用 Navbar 與公告 Bar）
- [x] 新增 `AuditableEntity` 稽核基底類別（建立/更新時間、建立/更新者、更新次數）
- [x] 新增 EC.B2E 後台管理系統（Port 5200）：商品 / 公告 / B2C 帳號 CRUD，8 語系介面，獨立管理員帳號
- [x] B2E 新增分類管理（8語系 CRUD）、首頁設定（精選商品 switch）、商品圖片上傳（wwwroot/uploads/products/）
- [x] B2C 首頁輪播優先顯示精選商品（`is_featured`），無精選時 fallback 至最新 4 筆
- [x] products 資料表新增 `is_featured` 欄位（DbSetup ALTER TABLE IF NOT EXISTS）
- [x] 修正非 Development 環境 Encryption salt 不一致導致登入 401 的問題
- [x] 補全所有頁面 8 語系覆蓋（包含 footer、title、hero 標語、badge、管理後台側邊欄等所有 UI 文字）
- [x] 加密金鑰改由 `appsettings.json` / 環境變數注入，不再硬編碼於原始碼
- [x] 新增 HTTP 安全標頭（X-Content-Type-Options / X-Frame-Options / X-XSS-Protection / Referrer-Policy / Permissions-Policy）
- [x] 新增登入 Rate Limiting（每分鐘 5 次，超過 429）
- [x] Swagger 限開發環境或設定啟用時才開放
- [x] CORS 改由設定檔控制允許來源
- [x] B2E API Request DTO 加入 Data Annotations 輸入驗證
- [x] Controller exception 不再洩漏堆疊資訊，改回通用錯誤訊息並記錄 log
- [ ] 加入 JWT 標準認證中介層
- [ ] 前端改為 Vue 3 SPA（Vite + Vue Router）
- [ ] 加入結帳 / 訂單管理功能
- [ ] Docker 容器化部署（含 PostgreSQL）
- [ ] 加入 EF Core 完整 Migration 歷史記錄
- [ ] 密碼改用 PBKDF2 / bcrypt（現為 SHA-256，建議升級）
