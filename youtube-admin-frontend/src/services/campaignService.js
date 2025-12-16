import api from './api';

const campaignService = {
  async getAll() {
    const response = await api.get('/adsensecampaigns');
    return response.data;
  },

  async getById(id) {
    const response = await api.get(`/adsensecampaigns/${id}`);
    return response.data;
  },

  async getActive() {
    const response = await api.get('/adsensecampaigns/active');
    return response.data;
  },

  async create(campaignData) {
    const response = await api.post('/adsensecampaigns', campaignData);
    return response.data;
  },

  async update(id, campaignData) {
    const response = await api.put(`/adsensecampaigns/${id}`, campaignData);
    return response.data;
  },

  async delete(id) {
    const response = await api.delete(`/adsensecampaigns/${id}`);
    return response.data;
  },
};

export default campaignService;
