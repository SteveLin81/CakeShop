// b2e-common.js ── B2E 後台共用 Vue Composition

const { createI18n } = VueI18n;

const b2eStoredLocale = localStorage.getItem('b2eLocale') || localStorage.getItem('locale') || 'zh-TW';
const b2eI18n = createI18n({ legacy: false, locale: b2eStoredLocale, messages: b2eMessages });

// ── 共用 Composition 函式 ────────────────────────────────────────────
function useB2eCommon() {
  const { ref, computed, onMounted, onBeforeUnmount } = Vue;
  const { t, locale } = b2eI18n.global;

  // ── 語言 ──────────────────────────────────────────────────────────
  const showLangMenu = ref(false);
  const langs = [
    { code: 'zh-TW', label: '繁體中文'      }, { code: 'zh-CN', label: '简体中文'       },
    { code: 'en',    label: 'English'       }, { code: 'ja',    label: '日本語'          },
    { code: 'th',    label: 'ภาษาไทย'      }, { code: 'ko',    label: '한국어'          },
    { code: 'vi',    label: 'Tiếng Việt'   }, { code: 'ms',    label: 'Bahasa Melayu'  },
  ];
  const currentLangLabel = computed(() => langs.find(l => l.code === locale.value)?.label || '');
  function setLocale(code) {
    locale.value = code;
    localStorage.setItem('b2eLocale', code);
    localStorage.setItem('locale', code);
  }
  function closeLangMenu(e) { if (!e.target.closest('.b2e-lang-btn')) showLangMenu.value = false; }
  onMounted(() => document.addEventListener('click', closeLangMenu));
  onBeforeUnmount(() => document.removeEventListener('click', closeLangMenu));

  // ── Auth ──────────────────────────────────────────────────────────
  const adminUsername = ref(localStorage.getItem('b2eUsername') || '');

  async function checkAuth() {
    const token = localStorage.getItem('b2eToken');
    if (!token) { location.href = '/b2e/login'; return; }
    const res = await b2eApi.validateToken(token);
    if (!res.isValid) { location.href = '/b2e/login'; }
  }

  function logout() {
    localStorage.removeItem('b2eToken');
    localStorage.removeItem('b2eUsername');
    location.href = '/b2e/login';
  }

  // ── Sidebar ───────────────────────────────────────────────────────
  const sidebarOpen = ref(false);

  // ── Toast ─────────────────────────────────────────────────────────
  const toast     = ref('');
  const toastType = ref('');
  let toastTimer;
  function showToast(msg, type = 'default') {
    toast.value     = msg;
    toastType.value = type;
    clearTimeout(toastTimer);
    toastTimer = setTimeout(() => { toast.value = ''; }, 3000);
  }

  return {
    t, locale, langs, currentLangLabel, setLocale, showLangMenu,
    adminUsername, checkAuth, logout,
    sidebarOpen, toast, toastType, showToast,
  };
}

// ── 空的商品表單預設值 ────────────────────────────────────────────────
function emptyProductForm() {
  return {
    name: '', nameEn: '', nameJa: '', nameZhCn: '', nameTh: '', nameKo: '', nameVi: '', nameMs: '',
    description: '', descriptionEn: '', descriptionJa: '', descriptionZhCn: '',
    descriptionTh: '', descriptionKo: '', descriptionVi: '', descriptionMs: '',
    price: 0, imageUrl: '', categoryId: null, isAvailable: true, isFeatured: false,
  };
}

// ── 空的公告表單預設值 ────────────────────────────────────────────────
function emptyAnnouncementForm() {
  return {
    content: '', contentEn: '', contentJa: '', contentZhCn: '',
    contentTh: '', contentKo: '', contentVi: '', contentMs: '',
    isActive: false,
  };
}

// ── 日期格式化工具 ────────────────────────────────────────────────────
function formatDate(d) {
  if (!d) return '';
  const dt = new Date(d);
  return `${dt.getFullYear()}-${String(dt.getMonth()+1).padStart(2,'0')}-${String(dt.getDate()).padStart(2,'0')} ${String(dt.getHours()).padStart(2,'0')}:${String(dt.getMinutes()).padStart(2,'0')}`;
}
