const API_BASE = '/api';

const api = {
  async request(method, url, body) {
    const headers = { 'Content-Type': 'application/json' };
    const token = localStorage.getItem('token');
    if (token) headers['Authorization'] = `Bearer ${token}`;
    const res = await fetch(`${API_BASE}${url}`, {
      method,
      headers,
      body: body ? JSON.stringify(body) : undefined
    });
    const data = await res.json();
    return data;
  },
  get: (url) => api.request('GET', url),
  post: (url, body) => api.request('POST', url, body),
  put: (url, body) => api.request('PUT', url, body),
  delete: (url) => api.request('DELETE', url),

  // 產品 API
  async getProducts() { return this.get('/product'); },
  async getCategories() { return this.get('/product/categories'); },
  async getProductsByCategory(id) { return this.get(`/product/category/${id}`); },

  // 認證 API
  async login(username, password) { return this.post('/auth/login', { username, password }); },
  async validateToken(token) { return this.post('/auth/validate', { token }); },

  // 購物車 API
  async getCart(sessionId) { return this.get(`/cart/${sessionId}`); },
  async addToCart(sessionId, productId, quantity = 1) {
    return this.post('/cart', { sessionId, productId, quantity });
  },
  async updateCartItem(sessionId, itemId, quantity) {
    return this.put(`/cart/${sessionId}/items/${itemId}`, { quantity });
  },
  async removeCartItem(sessionId, itemId) {
    return this.delete(`/cart/${sessionId}/items/${itemId}`);
  },
  async clearCart(sessionId) { return this.delete(`/cart/${sessionId}`); },

  // 聯絡表單 API
  async submitContact(form) { return this.post('/contact', form); },

  // 公告 API
  async getAnnouncement() { return this.get('/announcement'); }
};

// 取得或建立 sessionId
function getSessionId() {
  let sid = localStorage.getItem('sessionId');
  if (!sid) {
    sid = 'sess_' + Math.random().toString(36).slice(2) + Date.now().toString(36);
    localStorage.setItem('sessionId', sid);
  }
  return sid;
}

// 依語言取得產品名稱
function getProductName(p, locale) {
  if (locale === 'en') return p.nameEn || p.name;
  if (locale === 'ja') return p.nameJa || p.name;
  if (locale === 'zh-CN') return p.nameZhCn || p.name;
  return p.name;
}

// 依語言取得分類名稱
function getCategoryName(c, locale) {
  if (locale === 'en') return c.nameEn || c.name;
  if (locale === 'ja') return c.nameJa || c.name;
  if (locale === 'zh-CN') return c.nameZhCn || c.name;
  return c.name;
}

// 依語言取得產品描述
function getProductDesc(p, locale) {
  if (locale === 'en') return p.descriptionEn || p.description;
  if (locale === 'ja') return p.descriptionJa || p.description;
  if (locale === 'zh-CN') return p.descriptionZhCn || p.description;
  return p.description;
}
