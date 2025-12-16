import { useState, useEffect } from 'react';
import { X } from 'lucide-react';
import channelService from '../../services/channelService';

const VideoModal = ({ isOpen, onClose, onSave, video }) => {
  const [channels, setChannels] = useState([]);
  const [formData, setFormData] = useState({
    title: '',
    videoUrl: '',
    description: '',
    publishDate: '',
    viewCount: 0,
    likeCount: 0,
    commentCount: 0,
    duration: '',
    channelId: '',
    videoCategoryId: 1,
  });

  useEffect(() => {
    fetchChannels();
  }, []);

  useEffect(() => {
    if (video) {
      setFormData({
        title: video.title || '',
        videoUrl: video.videoUrl || '',
        description: video.description || '',
        publishDate: video.publishDate ? video.publishDate.split('T')[0] : '',
        viewCount: video.viewCount || 0,
        likeCount: video.likeCount || 0,
        commentCount: video.commentCount || 0,
        duration: video.duration || '',
        channelId: video.channelId || '',
        videoCategoryId: video.videoCategoryId || 1,
      });
    } else {
      setFormData({
        title: '',
        videoUrl: '',
        description: '',
        publishDate: '',
        viewCount: 0,
        likeCount: 0,
        commentCount: 0,
        duration: '',
        channelId: '',
        videoCategoryId: 1,
      });
    }
  }, [video]);

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
      viewCount: parseInt(formData.viewCount) || 0,
      likeCount: parseInt(formData.likeCount) || 0,
      commentCount: parseInt(formData.commentCount) || 0,
      channelId: parseInt(formData.channelId),
      videoCategoryId: parseInt(formData.videoCategoryId),
    });
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
      <div className="bg-white rounded-lg shadow-xl p-6 w-full max-w-2xl max-h-[90vh] overflow-y-auto">
        <div className="flex justify-between items-center mb-4">
          <h2 className="text-2xl font-bold text-gray-900">
            {video ? 'Editar Video' : 'Nuevo Video'}
          </h2>
          <button onClick={onClose} className="text-gray-500 hover:text-gray-700">
            <X className="w-6 h-6" />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Título *
            </label>
            <input
              type="text"
              name="title"
              value={formData.title}
              onChange={handleChange}
              required
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              placeholder="Título del video"
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
                  Debes crear un canal primero antes de poder crear videos
                </p>
              )}
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Duración
              </label>
              <input
                type="text"
                name="duration"
                value={formData.duration}
                onChange={handleChange}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                placeholder="HH:MM:SS"
              />
            </div>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              URL del Video *
            </label>
            <input
              type="url"
              name="videoUrl"
              value={formData.videoUrl}
              onChange={handleChange}
              required
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              placeholder="https://youtube.com/watch?v=..."
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
              placeholder="Descripción del video..."
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Fecha de Publicación
            </label>
            <input
              type="date"
              name="publishDate"
              value={formData.publishDate}
              onChange={handleChange}
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />
          </div>

          <div className="grid grid-cols-3 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Vistas
              </label>
              <input
                type="number"
                name="viewCount"
                value={formData.viewCount}
                onChange={handleChange}
                min="0"
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Likes
              </label>
              <input
                type="number"
                name="likeCount"
                value={formData.likeCount}
                onChange={handleChange}
                min="0"
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Comentarios
              </label>
              <input
                type="number"
                name="commentCount"
                value={formData.commentCount}
                onChange={handleChange}
                min="0"
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              />
            </div>
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
              {video ? 'Guardar Cambios' : 'Crear Video'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default VideoModal;
