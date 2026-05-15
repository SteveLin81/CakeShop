// common.js ── 所有頁面共用的 Vue Composition 設定

// i18n 初始化（在 i18n.js / api.js 載入後執行）
const { createI18n } = VueI18n;
const storedLocale = localStorage.getItem('locale') || 'zh-TW';
const i18n = createI18n({ legacy: false, locale: storedLocale, messages });

// ── 頁面轉場工具 ─────────────────────────────────────────────────────────
function dismissLoader() {
  const ls = document.getElementById('loading-screen');
  if (!ls) return;
  ls.classList.add('fade-out');
  setTimeout(() => { ls.style.display = 'none'; }, 450);
}
function applyExitTransition() {
  document.querySelectorAll('a[href]').forEach(el => {
    const href = el.getAttribute('href');
    if (href && href.startsWith('/') && !href.startsWith('//')) {
      el.addEventListener('click', e => {
        if (e.ctrlKey || e.metaKey || e.shiftKey) return;
        e.preventDefault();
        document.getElementById('app').classList.add('page-exit');
        setTimeout(() => { window.location.href = href; }, 230);
      });
    }
  });
}

// ── 共用 Composition 函式 ────────────────────────────────────────────────
function useCommonSetup() {
  const { ref, computed, onMounted, onBeforeUnmount } = Vue;
  const { t, locale } = i18n.global;

  // ── 語言 ──────────────────────────────────────────────────────────────
  const showLangMenu = ref(false);
  const langs = [
    { code: 'zh-TW', label: '繁體中文'      }, { code: 'zh-CN', label: '简体中文'       },
    { code: 'en',    label: 'English'       }, { code: 'ja',    label: '日本語'          },
    { code: 'th',    label: 'ภาษาไทย'      }, { code: 'ko',    label: '한국어'          },
    { code: 'vi',    label: 'Tiếng Việt'   }, { code: 'ms',    label: 'Bahasa Melayu'  },
  ];
  const currentLangLabel = computed(() => langs.find(l => l.code === locale.value)?.label || '');
  function setLocale(code) { locale.value = code; localStorage.setItem('locale', code); }

  // ── 捲動 / 選單 ────────────────────────────────────────────────────────
  const isScrolled = ref(false);
  function onScroll() { isScrolled.value = window.scrollY > 40; }
  function closeMenu(e) { if (!e.target.closest('.lang-btn')) showLangMenu.value = false; }
  onMounted(() => {
    window.addEventListener('scroll', onScroll);
    document.addEventListener('click', closeMenu);
  });
  onBeforeUnmount(() => {
    window.removeEventListener('scroll', onScroll);
    document.removeEventListener('click', closeMenu);
  });

  // ── 驗證 ──────────────────────────────────────────────────────────────
  const isLoggedIn  = ref(!!localStorage.getItem('token'));
  const username    = ref(localStorage.getItem('username') || '');
  const loginOpen   = ref(false);
  const loginForm   = ref({ username: '', password: '' });
  const loginErrors = ref({});
  const loginLoading = ref(false);
  const cartKey = computed(() => isLoggedIn.value ? username.value : null);

  async function doLogin() {
    loginErrors.value = {};
    if (!loginForm.value.username) loginErrors.value.username = t('auth.usernameReq');
    if (!loginForm.value.password) loginErrors.value.password = t('auth.passwordReq');
    if (Object.keys(loginErrors.value).length) return;
    loginLoading.value = true;
    try {
      const res = await api.login(loginForm.value.username, loginForm.value.password);
      if (res.success) {
        localStorage.setItem('token', res.token);
        localStorage.setItem('username', res.username);
        isLoggedIn.value = true;
        username.value = res.username;
        loginOpen.value = false;
        loginForm.value = { username: '', password: '' };
        await loadCart();
        showToast(t('auth.loginSuccess'));
      } else { loginErrors.value.global = t('auth.loginFailed'); }
    } catch { loginErrors.value.global = t('common.error'); }
    finally { loginLoading.value = false; }
  }

  function logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('username');
    isLoggedIn.value = false;
    username.value   = '';
    cartItems.value  = [];
    cartOpen.value   = false;
    showToast(t('auth.logoutSuccess'));
  }

  // ── 購物車 ────────────────────────────────────────────────────────────
  const cartOpen  = ref(false);
  const cartItems = ref([]);
  const cartTotal = computed(() => cartItems.value.reduce((s, i) => s + i.subTotal, 0));
  const cartCount = computed(() => cartItems.value.reduce((s, i) => s + i.quantity, 0));

  function getCartItemName(item) {
    const m = {
      'en': item.productNameEn, 'ja': item.productNameJa, 'zh-CN': item.productNameZhCn,
      'th': item.productNameTh, 'ko': item.productNameKo, 'vi':    item.productNameVi,
      'ms': item.productNameMs
    };
    return m[locale.value] || item.productName;
  }

  async function loadCart() {
    if (!cartKey.value) { cartItems.value = []; return; }
    try { const r = await api.getCart(cartKey.value); cartItems.value = r.items || []; } catch {}
  }

  function openCart() {
    if (!isLoggedIn.value) { loginOpen.value = true; return; }
    cartOpen.value = true;
  }

  async function addToCart(p) {
    if (!isLoggedIn.value) { loginOpen.value = true; return; }
    try {
      const r = await api.addToCart(cartKey.value, p.id);
      if (r.success) { cartItems.value = r.cart.items; showToast(t('cart.added')); }
    } catch { showToast(t('common.error')); }
  }

  async function updateQty(item, qty) {
    try { const r = await api.updateCartItem(cartKey.value, item.id, qty); if (r.success) cartItems.value = r.cart.items; } catch {}
  }

  async function removeItem(item) {
    try { const r = await api.removeCartItem(cartKey.value, item.id); if (r.success) cartItems.value = r.cart.items; } catch {}
  }

  async function clearCart() {
    try { await api.clearCart(cartKey.value); cartItems.value = []; } catch {}
  }

  // ── 公告 ──────────────────────────────────────────────────────────────
  const announcement        = ref(null);
  const announcementVisible = ref(false);
  const announcementContent = computed(() => {
    if (!announcement.value) return '';
    const m = {
      'en': announcement.value.contentEn, 'ja':    announcement.value.contentJa,
      'zh-CN': announcement.value.contentZhCn, 'th': announcement.value.contentTh,
      'ko': announcement.value.contentKo,  'vi':    announcement.value.contentVi,
      'ms': announcement.value.contentMs,
    };
    return m[locale.value] || announcement.value.content;
  });

  async function loadAnnouncement() {
    try {
      const ann = await api.getAnnouncement();
      if (ann && ann.isActive && !sessionStorage.getItem('announcementDismissed')) {
        announcement.value = ann;
        announcementVisible.value = true;
      }
    } catch {}
  }

  function dismissAnnouncement() {
    announcementVisible.value = false;
    sessionStorage.setItem('announcementDismissed', '1');
  }

  // ── Toast ─────────────────────────────────────────────────────────────
  const toast = ref('');
  let toastTimer;
  function showToast(msg) {
    toast.value = msg;
    clearTimeout(toastTimer);
    toastTimer = setTimeout(() => { toast.value = ''; }, 2800);
  }

  return {
    t, locale, langs, currentLangLabel, setLocale, showLangMenu, isScrolled,
    isLoggedIn, username, loginOpen, loginForm, loginErrors, loginLoading, doLogin, logout,
    cartKey, cartOpen, openCart, cartItems, cartTotal, cartCount, getCartItemName,
    loadCart, addToCart, updateQty, removeItem, clearCart,
    announcementVisible, announcementContent, loadAnnouncement, dismissAnnouncement,
    toast, showToast,
  };
}
