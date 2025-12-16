import { useState, useEffect } from 'react';
import { X } from 'lucide-react';
import channelService from '../../services/channelService';

const CampaignModal = ({ isOpen, onClose, onSave, campaign }) => {
  const [channels, setChannels] = useState([]);
  const [formData, setFormData] = useState({
    campaignName: '',
    description: '',
    startDate: '',
    endDate: '',
    budget: 0,
    currentSpent: 0,
    status: 'Active',
    adFormat: '',
    channelId: '',
  });

  useEffect(() => {
    fetchChannels();
  }, []);

  useEffect(() => {
    if (campaign) {
      setFormData({
        campaignName: campaign.campaignName || '',
        description: campaign.description || '',
        startDate: campaign.startDate ? campaign.startDate.split('T')[0] : '',
        endDate: campaign.endDate ? campaign.endDate.split('T')[0] : '',
        budget: campaign.budget || 0,
        currentSpent: campaign.currentSpent || 0,
        status: campaign.status || 'Active',
        adFormat: campaign.adFormat || '',
        channelId: campaign.channelId || '',
      });
    } else {
      setFormData({
        campaignName: '',
        description: '',
        startDate: '',
        endDate: '',
        budget: 0,
        currentSpent: 0,
        status: 'Active',
        adFormat: '',
        channelId: '',
      });
    }
  }, [campaign]);

  const fetchChannels = async () => {
    try {
      const data = await channelService.getAll();
      setChannels(data);
    } catch (error) {
      console.error('Error fetching channels:', error);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    onSave({
      ...formData,
      budget: parseFloat(formData.budget) || 0,
      currentSpent: parseFloat(formData.currentSpent) || 0,
      channelId: parseInt(formData.channelId),
    });
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
      <div className="bg-white rounded-lg shadow-xl p-6 w-full max-w-2xl max-h-[90vh] overflow-y-auto">
        <div className="flex justify-between items-center mb-4">
          <h2 className="text-2xl font-bold text-gray-900">
            {campaign ? 'Editar Campaña' : 'Nueva Campaña'}
          </h2>
          <button onClick={onClose} className="text-gray-500 hover:text-gray-700">
            <X className="w-6 h-6" />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Nombre de la Campaña *
            </label>
            <input
              type="text"
              name="campaignName"
              value={formData.campaignName}
              onChange={handleChange}
              required
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              placeholder="Nombre de la campaña"
            />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Canal *
              </label>
              <select
                name="channelId"
                value={formData.channelId}
                onChange={handleChange}
                required
                disabled={channels.length === 0}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent disabled:bg-gray-100 disabled:cursor-not-allowed"
              >
                <option value="">
                  {channels.length === 0 ? 'No tienes canales disponibles' : 'Seleccionar canal'}
                </option>
                {channels.map((channel) => (
                  <option key={channel.channelId} value={channel.channelId}>
                    {channel.channelName}
                  </option>
                ))}
              </select>
              {channels.length === 0 && (
                <p className="mt-1 text-sm text-red-600">
                  Debes crear un canal primero antes de crear campañas
                </p>
              )}
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Estado *
              </label>
              <select
                name="status"
                value={formData.status}
                onChange={handleChange}
                required
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              >
                <option value="Active">Activa</option>
                <option value="Paused">Pausada</option>
                <option value="Completed">Completada</option>
                <option value="Cancelled">Cancelada</option>
              </select>
            </div>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Descripción
            </label>
            <textarea
              name="description"
              value={formData.description}
              onChange={handleChange}
              rows="3"
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              placeholder="Descripción de la campaña..."
            />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Fecha de Inicio *
              </label>
              <input
                type="date"
                name="startDate"
                value={formData.startDate}
                onChange={handleChange}
                required
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Fecha de Fin *
              </label>
              <input
                type="date"
                name="endDate"
                value={formData.endDate}
                onChange={handleChange}
                required
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Presupuesto *
              </label>
              <input
                type="number"
                name="budget"
                value={formData.budget}
                onChange={handleChange}
                required
                min="0"
                step="0.01"
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                placeholder="0.00"
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Gasto Actual
              </label>
              <input
                type="number"
                name="currentSpent"
                value={formData.currentSpent}
                onChange={handleChange}
                min="0"
                step="0.01"
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                placeholder="0.00"
              />
            </div>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Formato de Anuncio *
            </label>
            <input
              type="text"
              name="adFormat"
              value={formData.adFormat}
              onChange={handleChange}
              required
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              placeholder="Ej: Display, Video, Display + Video"
            />
          </div>

          <div className="flex justify-end gap-3 mt-6">
            <button
              type="button"
              onClick={onClose}
              className="px-4 py-2 border border-gray-300 rounded-lg text-gray-700 hover:bg-gray-50"
            >
              Cancelar
            </button>
            <button
              type="submit"
              disabled={channels.length === 0}
              className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:bg-gray-400 disabled:cursor-not-allowed"
            >
              {campaign ? 'Guardar Cambios' : 'Crear Campaña'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default CampaignModal;
