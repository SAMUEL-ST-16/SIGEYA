import { useState, useEffect } from 'react';
import { X } from 'lucide-react';

const ChannelModal = ({ isOpen, onClose, onSave, channel }) => {
  const [formData, setFormData] = useState({
    channelName: '',
    channelUrl: '',
    subscriberCount: 0,
    description: '',
    isActive: true,
  });

  useEffect(() => {
    if (channel) {
      setFormData({
        channelName: channel.channelName || '',
        channelUrl: channel.channelUrl || '',
        subscriberCount: channel.subscriberCount || 0,
        description: channel.description || '',
        isActive: channel.isActive ?? true,
      });
    } else {
      setFormData({
        channelName: '',
        channelUrl: '',
        subscriberCount: 0,
        description: '',
        isActive: true,
      });
    }
  }, [channel]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData({
      ...formData,
      [name]: type === 'checkbox' ? checked : value,
    });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    onSave({
      ...formData,
      subscriberCount: parseInt(formData.subscriberCount) || 0,
    });
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div className="bg-white rounded-lg shadow-xl p-6 w-full max-w-md">
        <div className="flex justify-between items-center mb-4">
          <h2 className="text-2xl font-bold text-gray-900">
            {channel ? 'Editar Canal' : 'Nuevo Canal'}
          </h2>
          <button onClick={onClose} className="text-gray-500 hover:text-gray-700">
            <X className="w-6 h-6" />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Nombre del Canal *
            </label>
            <input
              type="text"
              name="channelName"
              value={formData.channelName}
              onChange={handleChange}
              required
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              placeholder="Mi Canal de YouTube"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              URL del Canal *
            </label>
            <input
              type="url"
              name="channelUrl"
              value={formData.channelUrl}
              onChange={handleChange}
              required
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              placeholder="https://youtube.com/@micanal"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Suscriptores
            </label>
            <input
              type="number"
              name="subscriberCount"
              value={formData.subscriberCount}
              onChange={handleChange}
              min="0"
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              placeholder="0"
            />
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
              placeholder="Descripción del canal..."
            />
          </div>

          <div className="flex items-center">
            <input
              type="checkbox"
              name="isActive"
              checked={formData.isActive}
              onChange={handleChange}
              className="w-4 h-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
            />
            <label className="ml-2 text-sm text-gray-700">Canal activo</label>
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
              className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
            >
              {channel ? 'Guardar Cambios' : 'Crear Canal'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default ChannelModal;
