import api from './api';

const authService = {
  async login(username, password) {
    console.log('authService.login - Enviando request...');
    const response = await api.post('/auth/login', {
      Username: username,
      Password: password
    });
    console.log('authService.login - Respuesta recibida:', response.data);
    if (response.data.token) {
      localStorage.setItem('token', response.data.token);
      // Store user data (excluding token)
      const userData = {
        userId: response.data.userId,
        username: response.data.username,
        email: response.data.email,
        roleName: response.data.roleName,
        firstName: response.data.firstName,
        lastName: response.data.lastName
      };
      console.log('authService.login - Guardando usuario:', userData);
      localStorage.setItem('user', JSON.stringify(userData));
    } else {
      console.error('authService.login - No se recibió token en la respuesta');
    }
    return response.data;
  },

  async register(userData) {
    const response = await api.post('/auth/register', userData);
    if (response.data.token) {
      localStorage.setItem('token', response.data.token);
      // Store user data (excluding token)
      const userInfo = {
        userId: response.data.userId,
        username: response.data.username,
        email: response.data.email,
        roleName: response.data.roleName,
        firstName: response.data.firstName,
        lastName: response.data.lastName
      };
      localStorage.setItem('user', JSON.stringify(userInfo));
    }
    return response.data;
  },

  async oauthLogin(token, provider) {
    console.log('authService.oauthLogin - Enviando request OAuth...');
    const response = await api.post('/auth/oauth-login', {
      Token: token,
      Provider: provider
    });
    console.log('authService.oauthLogin - Respuesta recibida:', response.data);
    if (response.data.token) {
      localStorage.setItem('token', response.data.token);
      // Store user data (excluding token)
      const userData = {
        userId: response.data.userId,
        username: response.data.username,
        email: response.data.email,
        roleName: response.data.roleName,
        firstName: response.data.firstName,
        lastName: response.data.lastName
      };
      console.log('authService.oauthLogin - Guardando usuario:', userData);
      localStorage.setItem('user', JSON.stringify(userData));
    } else {
      console.error('authService.oauthLogin - No se recibió token en la respuesta');
    }
    return response.data;
  },

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  },

  getCurrentUser() {
    const userStr = localStorage.getItem('user');
    if (!userStr || userStr === 'undefined' || userStr === 'null') {
      return null;
    }
    try {
      return JSON.parse(userStr);
    } catch (error) {
      console.error('Error parsing user from localStorage:', error);
      localStorage.removeItem('user');
      return null;
    }
  },

  getToken() {
    return localStorage.getItem('token');
  },

  isAuthenticated() {
    return !!this.getToken();
  },
};

export default authService;
