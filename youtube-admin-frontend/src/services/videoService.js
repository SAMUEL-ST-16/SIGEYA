import api from './api';

const videoService = {
  async getAll() {
    const response = await api.get('/videos');
    return response.data;
  },

  async getById(id) {
    const response = await api.get(`/videos/${id}`);
    return response.data;
  },

  async getByChannel(channelId) {
    const response = await api.get(`/videos/channel/${channelId}`);
    return response.data;
  },

  async create(videoData) {
    const response = await api.post('/videos', videoData);
    return response.data;
  },

  async update(id, videoData) {
    const response = await api.put(`/videos/${id}`, videoData);
    return response.data;
  },

  async delete(id) {
    const response = await api.delete(`/videos/${id}`);
    return response.data;
  },
};

export default videoService;
