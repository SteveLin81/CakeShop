# 🎂 Sweet Bakes 甜蜜烘焙坊

多語系蛋糕購物網站，後端採 .NET 8 四層架構，前端以 Vue 3 + Element Plus 實作，支援繁中、English、日本語、简中四種語言切換。

---

## 技術堆疊

| 層次 | 技術 |
|------|------|
| 後端框架 | ASP.NET Core 8 Web API |
| 語言 | C# 12 |
| 前端框架 | Vue 3（CDN）、Element Plus、Vue-i18n |
| 加密 | SHA-256 密碼雜湊 + AES-256-GCM Token |
| API 文件 | Swagger / Swashbuckle |
| 資料儲存 | 記憶體（Hardcoded，無資料庫依賴） |

---

## 專案架構

```
CakeShop/
├── CakeShop.Core/                  # 共用層（Models、DTOs、Interfaces）
│   ├── Models/
│   │   ├── Product.cs
│   │   ├── Category.cs
│   │   ├── User.cs
│   │   └── CartItem.cs
│   ├── DTOs/
│   │   ├── ProductDto.cs
│   │   ├── CategoryDto.cs
│   │   ├── CartDto.cs
│   │   ├── LoginRequest.cs
│   │   ├── LoginResponse.cs
│   │   ├── ContactFormDto.cs
│   │   └── AnnouncementDto.cs
│   └── Interfaces/
│       ├── IEncryptionService.cs
│       ├── IAuthService.cs
│       ├── IProductService.cs
│       ├── ICartService.cs
│       ├── IContactService.cs
│       ├── IAnnouncementService.cs
│       ├── IProductRepository.cs
│       ├── IUserRepository.cs
│       └── ICartRepository.cs
│
├── CakeShop.Business/              # 商業邏輯層
│   └── Services/
│       ├── EncryptionService.cs    # SHA-256 雜湊 + AES-256-GCM 加解密
│       ├── AuthService.cs          # 登入驗證、Token 產生
│       ├── ProductService.cs       # 商品查詢邏輯
│       ├── CartService.cs          # 購物車操作邏輯
│       ├── ContactService.cs       # 聯絡表單處理
│       └── AnnouncementService.cs  # 置頂公告管理
│
├── CakeShop.Infrastructure/        # 非商業邏輯層（資料存取）
│   └── Repositories/
│       ├── ProductRepository.cs    # 10 種蛋糕硬碼資料
│       ├── UserRepository.cs       # 使用者帳號硬碼資料
│       └── CartRepository.cs       # 記憶體購物車
│
├── CakeShop.Api/                   # 主站台層（輸入 / 呼叫 / 輸出）
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── ProductController.cs
│   │   ├── CartController.cs
│   │   ├── ContactController.cs
│   │   └── AnnouncementController.cs
│   ├── Program.cs                  # DI 註冊、Swagger、靜態檔案
│   └── wwwroot/                    # 前端靜態頁面
│       ├── index.html              # 首頁（Hero + Carousel + 精選商品）
│       ├── products.html           # 商品頁（分類篩選 + 搜尋）
│       ├── contact.html            # 聯絡我們（表單）
│       ├── css/style.css           # 自訂樣式（深綠主色調）
│       └── js/
│           ├── i18n.js             # 四語系翻譯定義
│           └── api.js              # API 呼叫工具函式
│
└── CakeShop.sln
```

### 架構說明

```
請求流程：
瀏覽器 → Controller（輸入/輸出）→ Service（商業邏輯）→ Repository（資料）

依賴方向：
Api → Business → Core ← Infrastructure
```

- **Controller**：只負責接收請求、呼叫 Service、回傳結果，不含任何業務邏輯
- **Service**：實作商業邏輯，依賴 Interface（全部介面定義在 Core）
- **Repository**：負責資料存取，目前以記憶體 List 模擬資料庫
- **Core**：純定義層，不依賴其他專案

---

## 環境需求

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- 瀏覽器（Chrome / Edge / Firefox 最新版）
- 不需要資料庫，無需額外安裝套件

---

## 啟動方式

### 1. 還原套件並建置

```bash
cd CakeShop
dotnet restore
dotnet build
```

### 2. 啟動 API 伺服器

```bash
cd CakeShop.Api
dotnet run
```

伺服器預設監聽 **http://localhost:5000**

### 3. 開啟網站

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

> 密碼以 SHA-256 雜湊儲存；登入後返回的 Token 以 AES-256-GCM 加密。

---

## API 端點一覽

### 認證

| 方法 | 路徑 | 說明 |
|------|------|------|
| POST | `/api/auth/login` | 登入，回傳加密 Token |
| POST | `/api/auth/validate` | 驗證 Token 是否有效 |

### 商品

| 方法 | 路徑 | 說明 |
|------|------|------|
| GET | `/api/product` | 取得全部商品 |
| GET | `/api/product/{id}` | 取得單一商品 |
| GET | `/api/product/categories` | 取得分類列表 |
| GET | `/api/product/category/{id}` | 依分類取得商品 |

### 購物車

| 方法 | 路徑 | 說明 |
|------|------|------|
| GET | `/api/cart/{sessionId}` | 取得購物車內容 |
| POST | `/api/cart` | 新增商品 |
| PUT | `/api/cart/{sessionId}/items/{itemId}` | 更新數量 |
| DELETE | `/api/cart/{sessionId}/items/{itemId}` | 移除單項 |
| DELETE | `/api/cart/{sessionId}` | 清空購物車 |

> 已登入時 `sessionId` 使用帳號名稱，確保購物車資料與帳號綁定。

### 聯絡 / 公告

| 方法 | 路徑 | 說明 |
|------|------|------|
| POST | `/api/contact` | 提交聯絡表單 |
| GET | `/api/announcement` | 取得置頂公告 |

---

## 前端功能

| 功能 | 說明 |
|------|------|
| 多語系 | 繁中 / English / 日本語 / 简中，語系設定存入 `localStorage` |
| 登入 | Modal 彈窗，Token 存入 `localStorage` |
| 購物車 | 需登入才能操作；登出後自動清空；資料綁定帳號 |
| 首頁輪播 | 商品清單倒排取4項，每4秒自動切換 |
| 置頂公告 | 來自後端 API，支援4語系，可按 ✕ 關閉（session 內不再顯示） |
| Loading 動畫 | 頁面切換時顯示深綠色 Loading Screen |
| 進/離場動畫 | Navbar 下滑 + 內容淡入；離頁時內容淡出上移 |
| RWD | 支援手機、平板、桌機 |

---

## 商品資料

硬碼於 `CakeShop.Infrastructure/Repositories/ProductRepository.cs`，共 10 種蛋糕：

| # | 商品名稱 | 分類 | 價格 |
|---|----------|------|------|
| 1 | 巧克力熔岩蛋糕 | 巧克力 | NT$280 |
| 2 | 草莓鮮奶油蛋糕 | 水果 | NT$350 |
| 3 | 抹茶紅豆蛋糕 | 日式 | NT$320 |
| 4 | 芒果慕斯蛋糕 | 水果 | NT$380 |
| 5 | 藍莓起司蛋糕 | 起司 | NT$420 |
| 6 | 黑森林蛋糕 | 巧克力 | NT$450 |
| 7 | 提拉米蘇 | 經典 | NT$360 |
| 8 | 紅絲絨蛋糕 | 經典 | NT$400 |
| 9 | 檸檬磅蛋糕 | 柑橘 | NT$290 |
| 10 | 焦糖蘋果塔 | 水果 | NT$340 |

---

## 加密機制

```
密碼儲存：SHA-256( password + salt ) → Base64
Token 格式：AES-256-GCM 加密的 "userId|username|expiresAt"
Token 結構：nonce(12 bytes) | tag(16 bytes) | ciphertext
```

密鑰由 `MasterSecret` 字串經 SHA-256 推導，實際部署時應替換為環境變數。

---

## 後續擴充建議

- [ ] 整合 PostgreSQL / SQL Server，替換記憶體 Repository
- [ ] 使用 `appsettings.json` 管理加密金鑰（注入 `IConfiguration`）
- [ ] 加入 JWT 標準認證中介層
- [ ] 前端改為 Vue 3 SPA（Vite + Vue Router）
- [ ] 加入結帳 / 訂單管理功能
- [ ] Docker 容器化部署
