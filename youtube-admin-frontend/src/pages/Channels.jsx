import { useState, useEffect } from 'react';
import { Youtube, Plus, Edit2, Trash2, Lock } from 'lucide-react';
import channelService from '../services/channelService';
import ChannelModal from '../components/modals/ChannelModal';
import { usePermissions } from '../hooks/usePermissions';
import { useAuth } from '../contexts/AuthContext';

const Channels = () => {
  const [channels, setChannels] = useState([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [selectedChannel, setSelectedChannel] = useState(null);
  const permissions = usePermissions();
  const { user } = useAuth();

  useEffect(() => {
    fetchChannels();
  }, []);

  const fetchChannels = async () => {
    try {
      const data = await channelService.getAll();
      setChannels(data);
    } catch (error) {
      console.error('Error:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleCreate = () => {
    setSelectedChannel(null);
    setModalOpen(true);
  };

  const handleEdit = (channel) => {
    setSelectedChannel(channel);
    setModalOpen(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('¿Estás seguro de que deseas eliminar este canal?')) {
      try {
        await channelService.delete(id);
        fetchChannels();
      } catch (error) {
        console.error('Error deleting channel:', error);
        alert('Error al eliminar el canal');
      }
    }
  };

  const handleSave = async (channelData) => {
    try {
      if (selectedChannel) {
        await channelService.update(selectedChannel.channelId, channelData);
      } else {
        await channelService.create(channelData);
      }
      setModalOpen(false);
      fetchChannels();
    } catch (error) {
      console.error('Error saving channel:', error);
      alert('Error al guardar el canal');
    }
  };

  if (loading) {
    return <div className="p-8 flex justify-center"><div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div></div>;
  }

  return (
    <div className="p-8">
      <div className="flex justify-between items-center mb-6">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">Canales de YouTube</h1>
          {(permissions.isPartner || permissions.isEmployee) && (
            <p className="text-sm text-gray-600 mt-1">Viendo tus canales</p>
          )}
          {permissions.isViewer && (
            <p className="text-sm text-gray-600 mt-1">Solo visualización</p>
          )}
        </div>
        {permissions.canCreateChannel && (
          <button
            onClick={handleCreate}
            className="bg-blue-600 text-white px-4 py-2 rounded-lg flex items-center hover:bg-blue-700"
          >
            <Plus className="w-5 h-5 mr-2" />
            Nuevo Canal
          </button>
        )}
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {channels.map((channel) => (
          <div key={channel.channelId} className="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition">
            <div className="flex items-center mb-4">
              <div className="w-12 h-12 bg-red-100 rounded-full flex items-center justify-center">
                <Youtube className="w-6 h-6 text-red-600" />
              </div>
              <div className="ml-4 flex-1">
                <h3 className="font-bold text-gray-900">{channel.channelName}</h3>
                <p className="text-sm text-gray-500">{channel.subscriberCount?.toLocaleString()} suscriptores</p>
              </div>
            </div>
            <p className="text-sm text-gray-600 mb-4 line-clamp-2">{channel.description || 'Sin descripción'}</p>
            <div className="flex items-center justify-between text-sm mb-4">
              <span className={`px-3 py-1 rounded-full ${channel.isActive ? 'bg-green-100 text-green-700' : 'bg-gray-100 text-gray-700'}`}>
                {channel.isActive ? 'Activo' : 'Inactivo'}
              </span>
              <span className="text-gray-500">{channel.totalVideos || 0} videos</span>
            </div>
            {(permissions.canEditChannel || permissions.canEditOwnChannel || permissions.canDeleteChannel) && (
              <div className="flex gap-2">
                {(permissions.canEditChannel || permissions.canEditOwnChannel) && (
                  <button
                    onClick={() => handleEdit(channel)}
                    className="flex-1 bg-blue-50 text-blue-600 px-3 py-2 rounded-lg flex items-center justify-center hover:bg-blue-100 transition"
                  >
                    <Edit2 className="w-4 h-4 mr-1" />
                    Editar
                  </button>
                )}
                {permissions.canDeleteChannel && (
                  <button
                    onClick={() => handleDelete(channel.channelId)}
                    className="flex-1 bg-red-50 text-red-600 px-3 py-2 rounded-lg flex items-center justify-center hover:bg-red-100 transition"
                  >
                    <Trash2 className="w-4 h-4 mr-1" />
                    Eliminar
                  </button>
                )}
              </div>
            )}
            {!permissions.canEditChannel && !permissions.canEditOwnChannel && !permissions.canDeleteChannel && (
              <div className="text-center py-2 text-gray-500 text-sm flex items-center justify-center">
                <Lock className="w-4 h-4 mr-1" />
                Solo lectura
              </div>
            )}
          </div>
        ))}
      </div>

      {channels.length === 0 && (
        <div className="text-center py-12">
          <Youtube className="w-16 h-16 text-gray-300 mx-auto mb-4" />
          <p className="text-gray-500">No hay canales registrados</p>
        </div>
      )}

      <ChannelModal
        isOpen={modalOpen}
        onClose={() => setModalOpen(false)}
        onSave={handleSave}
        channel={selectedChannel}
      />
    </div>
  );
};

export default Channels;
