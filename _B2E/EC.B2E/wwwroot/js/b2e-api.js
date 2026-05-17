const B2E_API = '/api/b2e';

const b2eApi = {
  async request(method, url, body) {
    const headers = { 'Content-Type': 'application/json' };
    const token = localStorage.getItem('b2eToken');
    if (token) headers['Authorization'] = `Bearer ${token}`;
    const res = await fetch(`${B2E_API}${url}`, {
      method,
      headers,
      body: body !== undefined ? JSON.stringify(body) : undefined,
    });
    const text = await res.text();
    if (!text) return { success: true };
    return JSON.parse(text);
  },

  get:    (url)        => b2eApi.request('GET',    url),
  post:   (url, body)  => b2eApi.request('POST',   url, body),
  put:    (url, body)  => b2eApi.request('PUT',    url, body),
  patch:  (url, body)  => b2eApi.request('PATCH',  url, body),
  delete: (url)        => b2eApi.request('DELETE', url),

  // ── 認證 ────────────────────────────────────────────────────────
  login:          (username, password) => b2eApi.post('/auth/login', { username, password }),
  validateToken:  (token)              => b2eApi.post('/auth/validate', { token }),
  getMe:          ()                   => b2eApi.get('/auth/me'),
  changePassword: (data)               => b2eApi.post('/auth/change-password', data),

  // ── 角色管理 ─────────────────────────────────────────────────────
  getRoles:    ()       => b2eApi.get('/roles'),
  getRole:     (id)     => b2eApi.get(`/roles/${id}`),
  createRole:  (data)   => b2eApi.post('/roles', data),
  updateRole:  (id, d)  => b2eApi.put(`/roles/${id}`, d),
  deleteRole:  (id)     => b2eApi.delete(`/roles/${id}`),

  // ── 後台帳號管理 ─────────────────────────────────────────────────
  getAdmins:    ()       => b2eApi.get('/admins'),
  getAdmin:     (id)     => b2eApi.get(`/admins/${id}`),
  createAdmin:  (data)   => b2eApi.post('/admins', data),
  updateAdmin:  (id, d)  => b2eApi.put(`/admins/${id}`, d),
  deleteAdmin:  (id)     => b2eApi.delete(`/admins/${id}`),

  // ── 商品 ────────────────────────────────────────────────────────
  getProducts:     ()       => b2eApi.get('/products'),
  getProduct:      (id)     => b2eApi.get(`/products/${id}`),
  getCategories:   ()       => b2eApi.get('/products/categories'),
  createProduct:   (data)   => b2eApi.post('/products', data),
  updateProduct:   (id, d)  => b2eApi.put(`/products/${id}`, d),
  deleteProduct:   (id)     => b2eApi.delete(`/products/${id}`),
  uploadProductImage: async (file) => {
    const headers = {};
    const token = localStorage.getItem('b2eToken');
    if (token) headers['Authorization'] = `Bearer ${token}`;
    const fd = new FormData(); fd.append('file', file);
    const res = await fetch(`${B2E_API}/products/upload-image`, { method: 'POST', headers, body: fd });
    return JSON.parse(await res.text());
  },

  // ── 分類 ────────────────────────────────────────────────────────
  getAllCategories:    ()       => b2eApi.get('/categories'),
  getCategory:        (id)     => b2eApi.get(`/categories/${id}`),
  createCategory:     (data)   => b2eApi.post('/categories', data),
  updateCategory:     (id, d)  => b2eApi.put(`/categories/${id}`, d),
  deleteCategory:     (id)     => b2eApi.delete(`/categories/${id}`),

  // ── 公告 ────────────────────────────────────────────────────────
  getAnnouncements:    ()      => b2eApi.get('/announcements'),
  getAnnouncement:     (id)    => b2eApi.get(`/announcements/${id}`),
  createAnnouncement:  (data)  => b2eApi.post('/announcements', data),
  updateAnnouncement:  (id, d) => b2eApi.put(`/announcements/${id}`, d),
  deleteAnnouncement:  (id)    => b2eApi.delete(`/announcements/${id}`),
  activateAnnouncement:(id)    => b2eApi.patch(`/announcements/${id}/activate`),

  // ── B2C 帳號 ─────────────────────────────────────────────────────
  getUsers:    ()       => b2eApi.get('/users'),
  getUser:     (id)     => b2eApi.get(`/users/${id}`),
  createUser:  (data)   => b2eApi.post('/users', data),
  updateUser:  (id, d)  => b2eApi.put(`/users/${id}`, d),
  deleteUser:  (id)     => b2eApi.delete(`/users/${id}`),
};
