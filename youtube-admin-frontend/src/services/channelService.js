import api from './api';

const channelService = {
  async getAll() {
    const response = await api.get('/youtubechannels');
    return response.data;
  },

  async getById(id) {
    const response = await api.get(`/youtubechannels/${id}`);
    return response.data;
  },

  async create(channelData) {
    const response = await api.post('/youtubechannels', channelData);
    return response.data;
  },

  async update(id, channelData) {
    const response = await api.put(`/youtubechannels/${id}`, channelData);
    return response.data;
  },

  async delete(id) {
    const response = await api.delete(`/youtubechannels/${id}`);
    return response.data;
  },
};

export default channelService;
