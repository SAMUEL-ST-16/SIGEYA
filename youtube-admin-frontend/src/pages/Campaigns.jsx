import { useState, useEffect } from 'react';
import { DollarSign, Plus, Edit2, Trash2 } from 'lucide-react';
import campaignService from '../services/campaignService';
import CampaignModal from '../components/modals/CampaignModal';
import { usePermissions } from '../hooks/usePermissions';

const Campaigns = () => {
  const [campaigns, setCampaigns] = useState([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [selectedCampaign, setSelectedCampaign] = useState(null);
  const permissions = usePermissions();

  useEffect(() => {
    fetchCampaigns();
  }, []);

  const fetchCampaigns = async () => {
    try {
      const data = await campaignService.getAll();
      setCampaigns(data);
    } catch (error) {
      console.error('Error:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleCreate = () => {
    setSelectedCampaign(null);
    setModalOpen(true);
  };

  const handleEdit = (campaign) => {
    setSelectedCampaign(campaign);
    setModalOpen(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('¿Estás seguro de que deseas eliminar esta campaña?')) {
      try {
        await campaignService.delete(id);
        fetchCampaigns();
      } catch (error) {
        console.error('Error deleting campaign:', error);
        alert('Error al eliminar la campaña');
      }
    }
  };

  const handleSave = async (campaignData) => {
    try {
      if (selectedCampaign) {
        await campaignService.update(selectedCampaign.campaignId, campaignData);
      } else {
        await campaignService.create(campaignData);
      }
      setModalOpen(false);
      fetchCampaigns();
    } catch (error) {
      console.error('Error saving campaign:', error);
      alert('Error al guardar la campaña');
    }
  };

  if (loading) {
    return <div className="p-8 flex justify-center"><div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div></div>;
  }

  return (
    <div className="p-8">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold text-gray-900">Campañas AdSense</h1>
        {permissions.canCreateCampaign && (
          <button
            onClick={handleCreate}
            className="bg-blue-600 text-white px-4 py-2 rounded-lg flex items-center hover:bg-blue-700"
          >
            <Plus className="w-5 h-5 mr-2" />
            Nueva Campaña
          </button>
        )}
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {campaigns.map((campaign) => (
          <div key={campaign.campaignId} className="bg-white rounded-lg shadow-md p-6">
            <div className="flex items-center justify-between mb-4">
              <h3 className="text-xl font-bold text-gray-900">{campaign.campaignName}</h3>
              <div className="flex items-center gap-2">
                <span className={`px-3 py-1 text-xs rounded-full ${
                  campaign.isActive ? 'bg-green-100 text-green-700' : 'bg-gray-100 text-gray-700'
                }`}>
                  {campaign.status}
                </span>
                {permissions.canEditCampaign && (
                  <button
                    onClick={() => handleEdit(campaign)}
                    className="text-blue-600 hover:text-blue-900"
                    title="Editar"
                  >
                    <Edit2 className="w-4 h-4" />
                  </button>
                )}
                {permissions.canDeleteCampaign && (
                  <button
                    onClick={() => handleDelete(campaign.campaignId)}
                    className="text-red-600 hover:text-red-900"
                    title="Eliminar"
                  >
                    <Trash2 className="w-4 h-4" />
                  </button>
                )}
              </div>
            </div>

            {campaign.description && (
              <p className="text-sm text-gray-600 mb-4">{campaign.description}</p>
            )}

            <div className="grid grid-cols-2 gap-4 mb-4">
              <div className="bg-gray-50 p-4 rounded-lg">
                <p className="text-xs text-gray-500 mb-1">Presupuesto</p>
                <p className="text-lg font-bold text-gray-900">${campaign.budget?.toLocaleString()}</p>
              </div>
              <div className="bg-gray-50 p-4 rounded-lg">
                <p className="text-xs text-gray-500 mb-1">Gastado</p>
                <p className="text-lg font-bold text-gray-900">${campaign.currentSpent?.toLocaleString()}</p>
              </div>
            </div>

            <div className="w-full bg-gray-200 rounded-full h-2">
              <div
                className="bg-blue-600 h-2 rounded-full"
                style={{ width: `${(campaign.currentSpent / campaign.budget) * 100}%` }}
              ></div>
            </div>

            <div className="mt-4 flex items-center justify-between text-sm text-gray-500">
              <span>Canal: {campaign.channelName}</span>
              <span>{campaign.adFormat}</span>
            </div>
          </div>
        ))}
      </div>

      {campaigns.length === 0 && (
        <div className="text-center py-12">
          <DollarSign className="w-16 h-16 text-gray-300 mx-auto mb-4" />
          <p className="text-gray-500">No hay campañas registradas</p>
        </div>
      )}

      <CampaignModal
        isOpen={modalOpen}
        onClose={() => setModalOpen(false)}
        onSave={handleSave}
        campaign={selectedCampaign}
      />
    </div>
  );
};

export default Campaigns;
