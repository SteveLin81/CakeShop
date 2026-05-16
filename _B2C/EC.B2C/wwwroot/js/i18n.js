const messages = {
  'zh-TW': {
    nav: {
      home: '首頁', products: '產品', contact: '聯絡我們',
      login: '登入', logout: '登出', cart: '購物車',
      welcome: '歡迎', langLabel: '繁體中文', brand: '甜蜜烘焙坊'
    },
    home: {
      heroTitle: '甜蜜烘焙坊',
      heroSubtitle: '每一口都是精心製作的藝術',
      heroEyebrow: '精緻手工糕點', featuredTag: '精選',
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
      usernameReq: '請輸入帳號', passwordReq: '請輸入密碼',
      register: '建立帳號', registerTitle: '建立帳號',
      email: '電子郵件', confirmPassword: '確認密碼',
      forgotPassword: '忘記密碼？', forgotPasswordTitle: '忘記密碼',
      forgotPasswordDesc: '請輸入您的信箱，我們將發送重設連結',
      sendResetLink: '發送重設連結',
      resetLinkSent: '郵件已發送',
      resetLinkSentDesc: '請查看您的信箱並點擊重設連結（有效期 1 小時）',
      resetPasswordTitle: '重設密碼', resetPassword: '重設密碼',
      newPassword: '新密碼',
      resetSuccess: '密碼重設成功！請使用新密碼登入',
      invalidToken: '連結無效或已過期',
      passwordMismatch: '兩次密碼不一致',
      backToHome: '返回首頁', backToLogin: '返回登入',
      emailRequired: '請輸入電子郵件',
      passwordTooShort: '密碼長度不足（至少 6 碼）'
    },
    common: { currency: 'NT$', loading: '載入中...', error: '發生錯誤，請稍後再試', close: '關閉',
              footerDesc: '精緻烘焙，每日現作。用最好的食材，帶給您最甜蜜的滋味。',
              footerCopyright: '© 2024 Sweet Bakes. All rights reserved.' }
  },

  'en': {
    nav: {
      home: 'Home', products: 'Products', contact: 'Contact',
      login: 'Login', logout: 'Logout', cart: 'Cart',
      welcome: 'Welcome', langLabel: 'English', brand: 'Sweet Bakes'
    },
    home: {
      heroTitle: 'Sweet Bakes',
      heroSubtitle: 'Every bite is a work of art',
      heroEyebrow: 'Artisan Pastry Shop', featuredTag: 'FEATURED',
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
      usernameReq: 'Please enter username', passwordReq: 'Please enter password',
      register: 'Register', registerTitle: 'Create Account',
      email: 'Email', confirmPassword: 'Confirm Password',
      forgotPassword: 'Forgot Password?', forgotPasswordTitle: 'Forgot Password',
      forgotPasswordDesc: 'Enter your email address and we\'ll send you a reset link',
      sendResetLink: 'Send Reset Link',
      resetLinkSent: 'Email Sent',
      resetLinkSentDesc: 'Please check your inbox and click the reset link (valid for 1 hour)',
      resetPasswordTitle: 'Reset Password', resetPassword: 'Reset Password',
      newPassword: 'New Password',
      resetSuccess: 'Password reset successfully! Please log in with your new password',
      invalidToken: 'The link is invalid or has expired',
      passwordMismatch: 'Passwords do not match',
      backToHome: 'Back to Home', backToLogin: 'Back to Login',
      emailRequired: 'Please enter your email',
      passwordTooShort: 'Password too short (at least 6 characters)'
    },
    common: { currency: 'NT$', loading: 'Loading...', error: 'An error occurred, please try again', close: 'Close',
              footerDesc: 'Artisan baking, made fresh daily. The finest ingredients for the sweetest moments.',
              footerCopyright: '© 2024 Sweet Bakes. All rights reserved.' }
  },

  'ja': {
    nav: {
      home: 'ホーム', products: '商品', contact: 'お問い合わせ',
      login: 'ログイン', logout: 'ログアウト', cart: 'カート',
      welcome: 'ようこそ', langLabel: '日本語', brand: 'スイートベイクス'
    },
    home: {
      heroTitle: 'スイートベイクス',
      heroSubtitle: '一口一口に職人の魂が込められています',
      heroEyebrow: '職人パティスリー', featuredTag: '注目',
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
      usernameReq: 'ユーザー名を入力してください', passwordReq: 'パスワードを入力してください',
      register: 'アカウント作成', registerTitle: 'アカウント作成',
      email: 'メールアドレス', confirmPassword: 'パスワード確認',
      forgotPassword: 'パスワードを忘れましたか？', forgotPasswordTitle: 'パスワードリセット',
      forgotPasswordDesc: 'メールアドレスを入力してください。リセットリンクをお送りします',
      sendResetLink: 'リセットリンクを送信',
      resetLinkSent: 'メールを送信しました',
      resetLinkSentDesc: '受信トレイをご確認ください。リセットリンクをクリックしてください（有効期間：1時間）',
      resetPasswordTitle: 'パスワードリセット', resetPassword: 'パスワードをリセット',
      newPassword: '新しいパスワード',
      resetSuccess: 'パスワードをリセットしました。新しいパスワードでログインしてください',
      invalidToken: 'リンクが無効または期限切れです',
      passwordMismatch: 'パスワードが一致しません',
      backToHome: 'ホームに戻る', backToLogin: 'ログインに戻る',
      emailRequired: 'メールアドレスを入力してください',
      passwordTooShort: 'パスワードが短すぎます（6文字以上）'
    },
    common: { currency: 'NT$', loading: '読み込み中...', error: 'エラーが発生しました。再度お試しください', close: '閉じる',
              footerDesc: '毎日手作りの本格スイーツ。最高の素材で最高の味をお届けします。',
              footerCopyright: '© 2024 Sweet Bakes. All rights reserved.' }
  },

  'zh-CN': {
    nav: {
      home: '首页', products: '产品', contact: '联系我们',
      login: '登录', logout: '退出', cart: '购物车',
      welcome: '欢迎', langLabel: '简体中文', brand: '甜蜜烘焙坊'
    },
    home: {
      heroTitle: '甜蜜烘焙坊',
      heroSubtitle: '每一口都是精心制作的艺术',
      heroEyebrow: '精致手工糕点', featuredTag: '精选',
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
      usernameReq: '请输入账号', passwordReq: '请输入密码',
      register: '建立账号', registerTitle: '建立账号',
      email: '电子邮件', confirmPassword: '确认密码',
      forgotPassword: '忘记密码？', forgotPasswordTitle: '忘记密码',
      forgotPasswordDesc: '请输入您的邮箱，我们将发送重置链接',
      sendResetLink: '发送重置链接',
      resetLinkSent: '邮件已发送',
      resetLinkSentDesc: '请查看您的邮箱并点击重置链接（有效期 1 小时）',
      resetPasswordTitle: '重置密码', resetPassword: '重置密码',
      newPassword: '新密码',
      resetSuccess: '密码重置成功！请使用新密码登录',
      invalidToken: '链接无效或已过期',
      passwordMismatch: '两次密码不一致',
      backToHome: '返回首页', backToLogin: '返回登录',
      emailRequired: '请输入电子邮件',
      passwordTooShort: '密码长度不足（至少 6 位）'
    },
    common: { currency: 'NT$', loading: '加载中...', error: '发生错误，请稍后再试', close: '关闭',
              footerDesc: '精致烘焙，每日现作。用最好的食材，带给您最甜蜜的滋味。',
              footerCopyright: '© 2024 Sweet Bakes. All rights reserved.' }
  },

  'th': {
    nav: {
      home: 'หน้าแรก', products: 'สินค้า', contact: 'ติดต่อเรา',
      login: 'เข้าสู่ระบบ', logout: 'ออกจากระบบ', cart: 'ตะกร้า',
      welcome: 'ยินดีต้อนรับ', langLabel: 'ภาษาไทย', brand: 'สวีทเบคส์'
    },
    home: {
      heroTitle: 'สวีทเบคส์', heroSubtitle: 'ทุกคำคือผลงานศิลปะ',
      heroEyebrow: 'ร้านขนมอาร์ติซาน', featuredTag: 'แนะนำ',
      heroBtn: 'ดูสินค้า', featuredTitle: 'เค้กแนะนำ',
      featuredSubtitle: 'วัตถุดิบคัดสรร ทำด้วยมือ',
      f1Title: 'วัตถุดิบสด', f1Desc: 'คัดสรรวัตถุดิบสดใหม่ทุกวัน',
      f2Title: 'ทำด้วยมือ', f2Desc: 'ผลิตโดยเชฟขนมผู้เชี่ยวชาญ',
      f3Title: 'ส่งไว', f3Desc: 'บรรจุพิเศษเพื่อความสดใหม่',
      viewAll: 'ดูทั้งหมด', carouselTitle: 'เค้กแนะนำ', carouselTag: 'HOT PICKS', announceDismiss: 'ปิด'
    },
    products: {
      title: 'เค้กทั้งหมด', all: 'ทั้งหมด',
      search: 'ค้นหาเค้ก...', noResult: 'ไม่พบเค้กที่ต้องการ',
      addToCart: 'เพิ่มในตะกร้า', details: 'รายละเอียด', perSlice: '/ ชิ้น'
    },
    cart: {
      title: 'ตะกร้าสินค้า', empty: 'ตะกร้าว่างเปล่า',
      total: 'รวม', checkout: 'ชำระเงิน',
      remove: 'ลบ', clear: 'ล้างตะกร้า', qty: 'จำนวน', subtotal: 'ยอดรวม',
      added: 'เพิ่มในตะกร้าแล้ว', updated: 'อัปเดตจำนวนแล้ว', itemCount: 'รายการ'
    },
    contact: {
      title: 'ติดต่อเรา', subtitle: 'หากมีคำถาม โปรดติดต่อเรา',
      name: 'ชื่อ', email: 'อีเมล', subject: 'หัวข้อ', message: 'ข้อความ',
      send: 'ส่งข้อความ',
      success: 'ส่งข้อความสำเร็จแล้ว เราจะตอบกลับภายใน 1-2 วันทำการ',
      nameReq: 'กรุณากรอกชื่อ', emailReq: 'กรุณากรอกอีเมล',
      emailInvalid: 'กรุณากรอกอีเมลที่ถูกต้อง',
      subjectReq: 'กรุณากรอกหัวข้อ', messageReq: 'กรุณากรอกข้อความ',
      infoTitle: 'ข้อมูลติดต่อ', address: 'เลขที่ 1 ถนนซินอี้ แขวงต้าอัน ไทเป',
      phone: '+886-2-2345-6789', hours: 'จันทร์-อาทิตย์ 09:00–21:00', email2: 'hello@sweetbakes.tw'
    },
    auth: {
      loginTitle: 'เข้าสู่ระบบ', username: 'ชื่อผู้ใช้', password: 'รหัสผ่าน', login: 'เข้าสู่ระบบ',
      loginSuccess: 'เข้าสู่ระบบสำเร็จ!', loginFailed: 'ชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้อง',
      logoutSuccess: 'ออกจากระบบแล้ว', usernameReq: 'กรุณากรอกชื่อผู้ใช้', passwordReq: 'กรุณากรอกรหัสผ่าน',
      register: 'สร้างบัญชี', registerTitle: 'สร้างบัญชีใหม่',
      email: 'อีเมล', confirmPassword: 'ยืนยันรหัสผ่าน',
      forgotPassword: 'ลืมรหัสผ่าน?', forgotPasswordTitle: 'ลืมรหัสผ่าน',
      forgotPasswordDesc: 'กรอกอีเมลของคุณ เราจะส่งลิงก์รีเซ็ตให้',
      sendResetLink: 'ส่งลิงก์รีเซ็ต',
      resetLinkSent: 'ส่งอีเมลแล้ว',
      resetLinkSentDesc: 'กรุณาตรวจสอบกล่องจดหมายและคลิกลิงก์รีเซ็ต (ใช้ได้ 1 ชั่วโมง)',
      resetPasswordTitle: 'รีเซ็ตรหัสผ่าน', resetPassword: 'รีเซ็ตรหัสผ่าน',
      newPassword: 'รหัสผ่านใหม่',
      resetSuccess: 'รีเซ็ตรหัสผ่านสำเร็จ! กรุณาเข้าสู่ระบบด้วยรหัสผ่านใหม่',
      invalidToken: 'ลิงก์ไม่ถูกต้องหรือหมดอายุ',
      passwordMismatch: 'รหัสผ่านไม่ตรงกัน',
      backToHome: 'กลับหน้าหลัก', backToLogin: 'กลับเข้าสู่ระบบ',
      emailRequired: 'กรุณากรอกอีเมล',
      passwordTooShort: 'รหัสผ่านสั้นเกินไป (อย่างน้อย 6 ตัวอักษร)'
    },
    common: { currency: 'NT$', loading: 'กำลังโหลด...', error: 'เกิดข้อผิดพลาด โปรดลองใหม่', close: 'ปิด',
              footerDesc: 'อบขนมสดใหม่ทุกวัน ด้วยวัตถุดิบที่ดีที่สุดสำหรับช่วงเวลาที่หวานที่สุด',
              footerCopyright: '© 2024 Sweet Bakes. All rights reserved.' }
  },

  'ko': {
    nav: {
      home: '홈', products: '상품', contact: '문의',
      login: '로그인', logout: '로그아웃', cart: '장바구니',
      welcome: '환영합니다', langLabel: '한국어', brand: '스위트 베이크스'
    },
    home: {
      heroTitle: '스위트 베이크스', heroSubtitle: '한 입 한 입이 예술입니다',
      heroEyebrow: '장인 제과점', featuredTag: '추천',
      heroBtn: '케이크 보기', featuredTitle: '추천 케이크',
      featuredSubtitle: '엄선한 재료, 장인 손수 제작',
      f1Title: '신선한 재료', f1Desc: '매일 엄선한 신선한 재료 사용',
      f2Title: '수제', f2Desc: '숙련된 파티시에가 직접 제작',
      f3Title: '빠른 배송', f3Desc: '특수 포장으로 신선도 유지',
      viewAll: '전체 보기', carouselTitle: '추천 케이크', carouselTag: 'HOT PICKS', announceDismiss: '닫기'
    },
    products: {
      title: '전체 케이크', all: '전체',
      search: '케이크 검색...', noResult: '케이크를 찾을 수 없습니다',
      addToCart: '장바구니 추가', details: '상세', perSlice: '/ 조각'
    },
    cart: {
      title: '장바구니', empty: '장바구니가 비어 있습니다',
      total: '합계', checkout: '결제', remove: '삭제', clear: '장바구니 비우기',
      qty: '수량', subtotal: '소계', added: '장바구니에 추가됨', updated: '수량 업데이트됨', itemCount: '개 상품'
    },
    contact: {
      title: '문의하기', subtitle: '궁금한 점이 있으시면 언제든지 문의해 주세요',
      name: '이름', email: '이메일', subject: '제목', message: '메시지',
      send: '메시지 보내기',
      success: '메시지가 전송되었습니다. 1-2 영업일 내에 답변 드리겠습니다.',
      nameReq: '이름을 입력해 주세요', emailReq: '이메일을 입력해 주세요',
      emailInvalid: '유효한 이메일을 입력해 주세요',
      subjectReq: '제목을 입력해 주세요', messageReq: '메시지를 입력해 주세요',
      infoTitle: '연락처 정보', address: '대만 타이베이시 다안구 신이로 4단 1호',
      phone: '+886-2-2345-6789', hours: '월-일 09:00–21:00', email2: 'hello@sweetbakes.tw'
    },
    auth: {
      loginTitle: '로그인', username: '아이디', password: '비밀번호', login: '로그인',
      loginSuccess: '로그인 성공!', loginFailed: '아이디 또는 비밀번호가 올바르지 않습니다',
      logoutSuccess: '로그아웃 되었습니다', usernameReq: '아이디를 입력해 주세요', passwordReq: '비밀번호를 입력해 주세요',
      register: '회원가입', registerTitle: '계정 만들기',
      email: '이메일', confirmPassword: '비밀번호 확인',
      forgotPassword: '비밀번호를 잊으셨나요?', forgotPasswordTitle: '비밀번호 찾기',
      forgotPasswordDesc: '이메일 주소를 입력하시면 재설정 링크를 보내드립니다',
      sendResetLink: '재설정 링크 전송',
      resetLinkSent: '이메일 전송 완료',
      resetLinkSentDesc: '받은편지함을 확인하고 재설정 링크를 클릭해 주세요 (유효기간: 1시간)',
      resetPasswordTitle: '비밀번호 재설정', resetPassword: '비밀번호 재설정',
      newPassword: '새 비밀번호',
      resetSuccess: '비밀번호가 재설정되었습니다! 새 비밀번호로 로그인해 주세요',
      invalidToken: '링크가 유효하지 않거나 만료되었습니다',
      passwordMismatch: '비밀번호가 일치하지 않습니다',
      backToHome: '홈으로 돌아가기', backToLogin: '로그인으로 돌아가기',
      emailRequired: '이메일을 입력해 주세요',
      passwordTooShort: '비밀번호가 너무 짧습니다 (최소 6자)'
    },
    common: { currency: 'NT$', loading: '로딩 중...', error: '오류가 발생했습니다. 다시 시도해 주세요', close: '닫기',
              footerDesc: '매일 신선하게 만드는 수제 베이킹. 최고의 재료로 최고의 맛을 선사합니다.',
              footerCopyright: '© 2024 Sweet Bakes. All rights reserved.' }
  },

  'vi': {
    nav: {
      home: 'Trang Chủ', products: 'Sản Phẩm', contact: 'Liên Hệ',
      login: 'Đăng Nhập', logout: 'Đăng Xuất', cart: 'Giỏ Hàng',
      welcome: 'Chào Mừng', langLabel: 'Tiếng Việt', brand: 'Sweet Bakes'
    },
    home: {
      heroTitle: 'Sweet Bakes', heroSubtitle: 'Mỗi miếng là một tác phẩm nghệ thuật',
      heroEyebrow: 'Tiệm Bánh Thủ Công', featuredTag: 'NỔI BẬT',
      heroBtn: 'Khám Phá', featuredTitle: 'Bánh Nổi Bật',
      featuredSubtitle: 'Nguyên liệu tuyển chọn, thủ công tinh tế',
      f1Title: 'Nguyên Liệu Tươi', f1Desc: 'Chọn lọc nguyên liệu tươi ngon mỗi ngày',
      f2Title: 'Thủ Công', f2Desc: 'Được làm bởi thợ bánh chuyên nghiệp',
      f3Title: 'Giao Nhanh', f3Desc: 'Đóng gói đặc biệt giữ bánh luôn tươi',
      viewAll: 'Xem Tất Cả', carouselTitle: 'Bánh Nổi Bật', carouselTag: 'HOT PICKS', announceDismiss: 'Đóng'
    },
    products: {
      title: 'Tất Cả Bánh', all: 'Tất Cả',
      search: 'Tìm kiếm bánh...', noResult: 'Không tìm thấy bánh',
      addToCart: 'Thêm Vào Giỏ', details: 'Chi Tiết', perSlice: '/ miếng'
    },
    cart: {
      title: 'Giỏ Hàng', empty: 'Giỏ hàng trống',
      total: 'Tổng', checkout: 'Thanh Toán', remove: 'Xoá', clear: 'Xoá Giỏ Hàng',
      qty: 'Số Lượng', subtotal: 'Tạm Tính', added: 'Đã thêm vào giỏ', updated: 'Đã cập nhật số lượng', itemCount: 'sản phẩm'
    },
    contact: {
      title: 'Liên Hệ', subtitle: 'Liên hệ với chúng tôi nếu bạn có câu hỏi',
      name: 'Họ Tên', email: 'Email', subject: 'Tiêu Đề', message: 'Nội Dung',
      send: 'Gửi Tin Nhắn',
      success: 'Tin nhắn đã được gửi! Chúng tôi sẽ phản hồi trong 1-2 ngày làm việc.',
      nameReq: 'Vui lòng nhập họ tên', emailReq: 'Vui lòng nhập email',
      emailInvalid: 'Email không hợp lệ',
      subjectReq: 'Vui lòng nhập tiêu đề', messageReq: 'Vui lòng nhập nội dung',
      infoTitle: 'Thông Tin Liên Hệ', address: 'Số 1, Đường Xinyi, Quận Da\'an, Đài Bắc',
      phone: '+886-2-2345-6789', hours: 'Thứ 2 - CN 09:00–21:00', email2: 'hello@sweetbakes.tw'
    },
    auth: {
      loginTitle: 'Đăng Nhập', username: 'Tên Đăng Nhập', password: 'Mật Khẩu', login: 'Đăng Nhập',
      loginSuccess: 'Đăng nhập thành công!', loginFailed: 'Tên đăng nhập hoặc mật khẩu không đúng',
      logoutSuccess: 'Đã đăng xuất', usernameReq: 'Vui lòng nhập tên đăng nhập', passwordReq: 'Vui lòng nhập mật khẩu',
      register: 'Đăng ký', registerTitle: 'Tạo tài khoản',
      email: 'Email', confirmPassword: 'Xác nhận mật khẩu',
      forgotPassword: 'Quên mật khẩu?', forgotPasswordTitle: 'Quên mật khẩu',
      forgotPasswordDesc: 'Nhập địa chỉ email của bạn, chúng tôi sẽ gửi liên kết đặt lại mật khẩu',
      sendResetLink: 'Gửi liên kết đặt lại',
      resetLinkSent: 'Email đã được gửi',
      resetLinkSentDesc: 'Vui lòng kiểm tra hộp thư và nhấp vào liên kết đặt lại (có hiệu lực 1 giờ)',
      resetPasswordTitle: 'Đặt lại mật khẩu', resetPassword: 'Đặt lại mật khẩu',
      newPassword: 'Mật khẩu mới',
      resetSuccess: 'Đặt lại mật khẩu thành công! Vui lòng đăng nhập bằng mật khẩu mới',
      invalidToken: 'Liên kết không hợp lệ hoặc đã hết hạn',
      passwordMismatch: 'Mật khẩu không khớp',
      backToHome: 'Về trang chủ', backToLogin: 'Quay lại đăng nhập',
      emailRequired: 'Vui lòng nhập email',
      passwordTooShort: 'Mật khẩu quá ngắn (tối thiểu 6 ký tự)'
    },
    common: { currency: 'NT$', loading: 'Đang tải...', error: 'Có lỗi xảy ra, vui lòng thử lại', close: 'Đóng',
              footerDesc: 'Bánh nướng thủ công, làm tươi mỗi ngày. Nguyên liệu tốt nhất cho những khoảnh khắc ngọt ngào nhất.',
              footerCopyright: '© 2024 Sweet Bakes. All rights reserved.' }
  },

  'ms': {
    nav: {
      home: 'Laman Utama', products: 'Produk', contact: 'Hubungi Kami',
      login: 'Log Masuk', logout: 'Log Keluar', cart: 'Troli',
      welcome: 'Selamat Datang', langLabel: 'Bahasa Melayu', brand: 'Sweet Bakes'
    },
    home: {
      heroTitle: 'Sweet Bakes', heroSubtitle: 'Setiap gigitan adalah karya seni',
      heroEyebrow: 'Kedai Pastri Artisan', featuredTag: 'PILIHAN',
      heroBtn: 'Terokai Kek', featuredTitle: 'Kek Pilihan',
      featuredSubtitle: 'Bahan terpilih, buatan tangan berkualiti',
      f1Title: 'Bahan Segar', f1Desc: 'Bahan tempatan segar dipilih setiap hari',
      f2Title: 'Buatan Tangan', f2Desc: 'Dibuat oleh pembuat pastri berpengalaman',
      f3Title: 'Penghantaran Cepat', f3Desc: 'Pembungkusan khas untuk kesegaran kek',
      viewAll: 'Lihat Semua', carouselTitle: 'Kek Pilihan', carouselTag: 'HOT PICKS', announceDismiss: 'Tutup'
    },
    products: {
      title: 'Semua Kek', all: 'Semua',
      search: 'Cari kek...', noResult: 'Tiada kek dijumpai',
      addToCart: 'Tambah ke Troli', details: 'Butiran', perSlice: '/ hirisan'
    },
    cart: {
      title: 'Troli Belanja', empty: 'Troli kosong',
      total: 'Jumlah', checkout: 'Daftar Keluar', remove: 'Buang', clear: 'Kosongkan Troli',
      qty: 'Kuantiti', subtotal: 'Subjumlah', added: 'Ditambah ke troli', updated: 'Kuantiti dikemas kini', itemCount: 'item'
    },
    contact: {
      title: 'Hubungi Kami', subtitle: 'Jangan ragu untuk menghubungi kami',
      name: 'Nama', email: 'E-mel', subject: 'Subjek', message: 'Mesej',
      send: 'Hantar Mesej',
      success: 'Mesej berjaya dihantar! Kami akan membalas dalam 1-2 hari bekerja.',
      nameReq: 'Sila masukkan nama', emailReq: 'Sila masukkan e-mel',
      emailInvalid: 'Sila masukkan e-mel yang sah',
      subjectReq: 'Sila masukkan subjek', messageReq: 'Sila masukkan mesej',
      infoTitle: 'Maklumat Hubungan', address: 'No. 1, Seksyen 4, Jalan Xinyi, Daerah Da\'an, Taipei',
      phone: '+886-2-2345-6789', hours: 'Isnin-Ahad 09:00–21:00', email2: 'hello@sweetbakes.tw'
    },
    auth: {
      loginTitle: 'Log Masuk', username: 'Nama Pengguna', password: 'Kata Laluan', login: 'Log Masuk',
      loginSuccess: 'Berjaya log masuk!', loginFailed: 'Nama pengguna atau kata laluan tidak sah',
      logoutSuccess: 'Berjaya log keluar', usernameReq: 'Sila masukkan nama pengguna', passwordReq: 'Sila masukkan kata laluan',
      register: 'Daftar', registerTitle: 'Buat Akaun',
      email: 'E-mel', confirmPassword: 'Sahkan Kata Laluan',
      forgotPassword: 'Lupa Kata Laluan?', forgotPasswordTitle: 'Lupa Kata Laluan',
      forgotPasswordDesc: 'Masukkan alamat e-mel anda, kami akan menghantar pautan tetapan semula',
      sendResetLink: 'Hantar Pautan Tetapan Semula',
      resetLinkSent: 'E-mel Dihantar',
      resetLinkSentDesc: 'Sila semak peti masuk anda dan klik pautan tetapan semula (sah selama 1 jam)',
      resetPasswordTitle: 'Tetapkan Semula Kata Laluan', resetPassword: 'Tetapkan Semula',
      newPassword: 'Kata Laluan Baharu',
      resetSuccess: 'Kata laluan berjaya ditetapkan semula! Sila log masuk dengan kata laluan baharu',
      invalidToken: 'Pautan tidak sah atau telah tamat tempoh',
      passwordMismatch: 'Kata laluan tidak sepadan',
      backToHome: 'Kembali ke Laman Utama', backToLogin: 'Kembali ke Log Masuk',
      emailRequired: 'Sila masukkan e-mel',
      passwordTooShort: 'Kata laluan terlalu pendek (sekurang-kurangnya 6 aksara)'
    },
    common: { currency: 'NT$', loading: 'Memuatkan...', error: 'Ralat berlaku, sila cuba lagi', close: 'Tutup',
              footerDesc: 'Bakar artisan, dibuat segar setiap hari. Bahan terbaik untuk momen paling manis.',
              footerCopyright: '© 2024 Sweet Bakes. All rights reserved.' }
  }
};
