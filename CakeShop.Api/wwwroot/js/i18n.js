const messages = {
  'zh-TW': {
    nav: {
      home: '首頁', products: '產品', contact: '聯絡我們',
      login: '登入', logout: '登出', cart: '購物車',
      welcome: '歡迎', langLabel: '繁體中文'
    },
    home: {
      heroTitle: '甜蜜烘焙坊',
      heroSubtitle: '每一口都是精心製作的藝術',
      heroBtn: '探索蛋糕',
      featuredTitle: '精選蛋糕',
      featuredSubtitle: '嚴選頂級食材，職人手工製作',
      f1Title: '新鮮食材', f1Desc: '每日精選最新鮮的在地食材',
      f2Title: '職人手作', f2Desc: '由資深甜點師親手精心製作',
      f3Title: '快速配送', f3Desc: '保溫包裝，確保蛋糕最佳狀態',
      viewAll: '查看全部',
      carouselTitle: '精選輪播', carouselTag: 'HOT PICKS', announceDismiss: '關閉公告'
    },
    products: {
      title: '全部蛋糕', all: '全部',
      search: '搜尋蛋糕名稱...', noResult: '找不到符合的蛋糕',
      addToCart: '加入購物車', details: '詳細資訊', perSlice: '/ 片'
    },
    cart: {
      title: '購物車', empty: '購物車是空的',
      total: '合計', checkout: '結帳',
      remove: '移除', clear: '清空購物車',
      qty: '數量', subtotal: '小計',
      added: '已加入購物車', updated: '數量已更新',
      itemCount: '件商品'
    },
    contact: {
      title: '聯絡我們',
      subtitle: '有任何問題或需求，歡迎與我們聯繫',
      name: '姓名', email: '電子郵件',
      subject: '主旨', message: '訊息內容',
      send: '發送訊息',
      success: '訊息已成功發送，我們將於 1-2 個工作天內回覆您！',
      nameReq: '請輸入姓名', emailReq: '請輸入電子郵件',
      emailInvalid: '請輸入有效的電子郵件',
      subjectReq: '請輸入主旨', messageReq: '請輸入訊息內容',
      infoTitle: '聯絡資訊',
      address: '台北市大安區信義路四段 1 號',
      phone: '02-2345-6789',
      hours: '週一至週日 09:00–21:00',
      email2: 'hello@sweetbakes.tw'
    },
    auth: {
      loginTitle: '會員登入',
      username: '帳號', password: '密碼', login: '登入',
      loginSuccess: '登入成功！歡迎回來',
      loginFailed: '帳號或密碼錯誤，請重新輸入',
      logoutSuccess: '已成功登出',
      usernameReq: '請輸入帳號', passwordReq: '請輸入密碼'
    },
    common: { currency: 'NT$', loading: '載入中...', error: '發生錯誤，請稍後再試', close: '關閉' }
  },

  'en': {
    nav: {
      home: 'Home', products: 'Products', contact: 'Contact',
      login: 'Login', logout: 'Logout', cart: 'Cart',
      welcome: 'Welcome', langLabel: 'English'
    },
    home: {
      heroTitle: 'Sweet Bakes',
      heroSubtitle: 'Every bite is a work of art',
      heroBtn: 'Explore Cakes',
      featuredTitle: 'Featured Cakes',
      featuredSubtitle: 'Premium ingredients, handcrafted with passion',
      f1Title: 'Fresh Ingredients', f1Desc: 'Daily-selected local produce',
      f2Title: 'Handcrafted', f2Desc: 'Made by experienced pastry chefs',
      f3Title: 'Fast Delivery', f3Desc: 'Insulated packaging keeps cakes perfect',
      viewAll: 'View All',
      carouselTitle: 'Featured Picks', carouselTag: 'HOT PICKS', announceDismiss: 'Close'
    },
    products: {
      title: 'All Cakes', all: 'All',
      search: 'Search cakes...', noResult: 'No cakes found',
      addToCart: 'Add to Cart', details: 'Details', perSlice: '/ slice'
    },
    cart: {
      title: 'Shopping Cart', empty: 'Your cart is empty',
      total: 'Total', checkout: 'Checkout',
      remove: 'Remove', clear: 'Clear Cart',
      qty: 'Qty', subtotal: 'Subtotal',
      added: 'Added to cart', updated: 'Quantity updated',
      itemCount: 'items'
    },
    contact: {
      title: 'Contact Us',
      subtitle: 'Feel free to reach out with any questions',
      name: 'Name', email: 'Email',
      subject: 'Subject', message: 'Message',
      send: 'Send Message',
      success: 'Message sent! We\'ll get back to you within 1-2 business days.',
      nameReq: 'Please enter your name', emailReq: 'Please enter your email',
      emailInvalid: 'Please enter a valid email',
      subjectReq: 'Please enter a subject', messageReq: 'Please enter a message',
      infoTitle: 'Contact Information',
      address: 'No. 1, Sec. 4, Xinyi Rd., Da\'an Dist., Taipei',
      phone: '+886-2-2345-6789',
      hours: 'Mon–Sun 09:00–21:00',
      email2: 'hello@sweetbakes.tw'
    },
    auth: {
      loginTitle: 'Member Login',
      username: 'Username', password: 'Password', login: 'Login',
      loginSuccess: 'Login successful! Welcome back',
      loginFailed: 'Invalid username or password',
      logoutSuccess: 'Logged out successfully',
      usernameReq: 'Please enter username', passwordReq: 'Please enter password'
    },
    common: { currency: 'NT$', loading: 'Loading...', error: 'An error occurred, please try again', close: 'Close' }
  },

  'ja': {
    nav: {
      home: 'ホーム', products: '商品', contact: 'お問い合わせ',
      login: 'ログイン', logout: 'ログアウト', cart: 'カート',
      welcome: 'ようこそ', langLabel: '日本語'
    },
    home: {
      heroTitle: 'スイートベイクス',
      heroSubtitle: '一口一口に職人の魂が込められています',
      heroBtn: 'ケーキを探す',
      featuredTitle: '人気のケーキ',
      featuredSubtitle: '厳選素材、職人手作り',
      f1Title: '新鮮素材', f1Desc: '毎日厳選した地元の新鮮素材を使用',
      f2Title: '職人手作り', f2Desc: '経験豊富なパティシエが丁寧に手作り',
      f3Title: '迅速配送', f3Desc: '保冷包装でケーキを最高の状態でお届け',
      viewAll: 'すべて見る',
      carouselTitle: 'おすすめ', carouselTag: 'HOT PICKS', announceDismiss: '閉じる'
    },
    products: {
      title: 'すべてのケーキ', all: 'すべて',
      search: 'ケーキを検索...', noResult: '該当するケーキが見つかりません',
      addToCart: 'カートに追加', details: '詳細', perSlice: '/ スライス'
    },
    cart: {
      title: 'ショッピングカート', empty: 'カートは空です',
      total: '合計', checkout: 'レジへ進む',
      remove: '削除', clear: 'カートを空にする',
      qty: '数量', subtotal: '小計',
      added: 'カートに追加しました', updated: '数量を更新しました',
      itemCount: '点の商品'
    },
    contact: {
      title: 'お問い合わせ',
      subtitle: 'ご質問やご要望がありましたらお気軽にご連絡ください',
      name: 'お名前', email: 'メールアドレス',
      subject: '件名', message: 'メッセージ',
      send: '送信',
      success: 'メッセージを受け付けました。1〜2営業日以内にご返信いたします。',
      nameReq: 'お名前を入力してください', emailReq: 'メールアドレスを入力してください',
      emailInvalid: '有効なメールアドレスを入力してください',
      subjectReq: '件名を入力してください', messageReq: 'メッセージを入力してください',
      infoTitle: '連絡先情報',
      address: '台北市大安区信義路四段 1 号',
      phone: '+886-2-2345-6789',
      hours: '月〜日 09:00–21:00',
      email2: 'hello@sweetbakes.tw'
    },
    auth: {
      loginTitle: 'ログイン',
      username: 'ユーザー名', password: 'パスワード', login: 'ログイン',
      loginSuccess: 'ログインしました！',
      loginFailed: 'ユーザー名またはパスワードが間違っています',
      logoutSuccess: 'ログアウトしました',
      usernameReq: 'ユーザー名を入力してください', passwordReq: 'パスワードを入力してください'
    },
    common: { currency: 'NT$', loading: '読み込み中...', error: 'エラーが発生しました。再度お試しください', close: '閉じる' }
  },

  'zh-CN': {
    nav: {
      home: '首页', products: '产品', contact: '联系我们',
      login: '登录', logout: '退出', cart: '购物车',
      welcome: '欢迎', langLabel: '简体中文'
    },
    home: {
      heroTitle: '甜蜜烘焙坊',
      heroSubtitle: '每一口都是精心制作的艺术',
      heroBtn: '探索蛋糕',
      featuredTitle: '精选蛋糕',
      featuredSubtitle: '严选顶级食材，职人手工制作',
      f1Title: '新鲜食材', f1Desc: '每日精选最新鲜的本地食材',
      f2Title: '职人手作', f2Desc: '由资深甜点师亲手精心制作',
      f3Title: '快速配送', f3Desc: '保温包装，确保蛋糕最佳状态',
      viewAll: '查看全部',
      carouselTitle: '精选轮播', carouselTag: 'HOT PICKS', announceDismiss: '关闭公告'
    },
    products: {
      title: '全部蛋糕', all: '全部',
      search: '搜索蛋糕名称...', noResult: '找不到符合的蛋糕',
      addToCart: '加入购物车', details: '详细信息', perSlice: '/ 片'
    },
    cart: {
      title: '购物车', empty: '购物车是空的',
      total: '合计', checkout: '结账',
      remove: '移除', clear: '清空购物车',
      qty: '数量', subtotal: '小计',
      added: '已加入购物车', updated: '数量已更新',
      itemCount: '件商品'
    },
    contact: {
      title: '联系我们',
      subtitle: '有任何问题或需求，欢迎与我们联系',
      name: '姓名', email: '电子邮件',
      subject: '主题', message: '消息内容',
      send: '发送消息',
      success: '消息已成功发送，我们将于 1-2 个工作日内回复您！',
      nameReq: '请输入姓名', emailReq: '请输入电子邮件',
      emailInvalid: '请输入有效的电子邮件',
      subjectReq: '请输入主题', messageReq: '请输入消息内容',
      infoTitle: '联系信息',
      address: '台北市大安区信义路四段 1 号',
      phone: '02-2345-6789',
      hours: '周一至周日 09:00–21:00',
      email2: 'hello@sweetbakes.tw'
    },
    auth: {
      loginTitle: '会员登录',
      username: '账号', password: '密码', login: '登录',
      loginSuccess: '登录成功！欢迎回来',
      loginFailed: '账号或密码错误，请重新输入',
      logoutSuccess: '已成功退出',
      usernameReq: '请输入账号', passwordReq: '请输入密码'
    },
    common: { currency: 'NT$', loading: '加载中...', error: '发生错误，请稍后再试', close: '关闭' }
  }
};
