import { useState, useEffect } from 'react';
import { Youtube, Video, DollarSign, CheckSquare, TrendingUp } from 'lucide-react';
import channelService from '../services/channelService';
import videoService from '../services/videoService';
import campaignService from '../services/campaignService';
import taskService from '../services/taskService';
import { usePermissions } from '../hooks/usePermissions';
import { useAuth } from '../contexts/AuthContext';

const StatCard = ({ title, value, icon: Icon, color, trend }) => (
  <div className="bg-white rounded-lg shadow-md p-6">
    <div className="flex items-center justify-between">
      <div>
        <p className="text-sm text-gray-600 font-medium">{title}</p>
        <p className="text-3xl font-bold text-gray-900 mt-2">{value}</p>
        {trend && (
          <p className="text-sm text-green-600 mt-2 flex items-center">
            <TrendingUp className="w-4 h-4 mr-1" />
            {trend}
          </p>
        )}
      </div>
      <div className={`w-14 h-14 rounded-full flex items-center justify-center ${color}`}>
        <Icon className="w-7 h-7 text-white" />
      </div>
    </div>
  </div>
);

const Dashboard = () => {
  const [stats, setStats] = useState({
    channels: 0,
    videos: 0,
    campaigns: 0,
    tasks: 0,
  });
  const [loading, setLoading] = useState(true);
  const permissions = usePermissions();
  const { user } = useAuth();

  useEffect(() => {
    const fetchStats = async () => {
      try {
        // Fetch solo los recursos que el usuario puede ver
        const promises = [];

        // Canales y Videos: Admin, Partner, ContentManager, Viewer
        if (permissions.canViewChannels) {
          promises.push(channelService.getAll());
          promises.push(videoService.getAll());
        } else {
          promises.push(Promise.resolve([]));
          promises.push(Promise.resolve([]));
        }

        // Campañas: Admin, Partner, Employee, Viewer
        if (permissions.canViewCampaigns) {
          promises.push(campaignService.getAll());
        } else {
          promises.push(Promise.resolve([]));
        }

        // Tareas: Admin, Partner, Employee, Viewer
        if (permissions.canViewTasks) {
          promises.push(taskService.getAll());
        } else {
          promises.push(Promise.resolve([]));
        }

        const [channels, videos, campaigns, tasks] = await Promise.all(promises);

        setStats({
          channels: channels.length,
          videos: videos.length,
          campaigns: campaigns.length,
          tasks: tasks.length,
        });
      } catch (error) {
        console.error('Error fetching stats:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchStats();
  }, [permissions]);

  if (loading) {
    return (
      <div className="flex items-center justify-center h-screen">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  return (
    <div className="p-8">
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900">Dashboard</h1>
        <p className="text-gray-600 mt-2">Bienvenido, {user?.firstName} {user?.lastName} ({user?.roleName})</p>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        {permissions.canViewChannels && (
          <StatCard
            title="Canales de YouTube"
            value={stats.channels}
            icon={Youtube}
            color="bg-red-500"
            trend={permissions.isContentManager ? "Tus canales" : "+12% este mes"}
          />
        )}
        {permissions.canViewVideos && (
          <StatCard
            title="Videos Publicados"
            value={stats.videos}
            icon={Video}
            color="bg-blue-500"
            trend={permissions.isContentManager ? "Tus videos" : "+8% este mes"}
          />
        )}
        {permissions.canViewCampaigns && (
          <StatCard
            title="Campañas AdSense"
            value={stats.campaigns}
            icon={DollarSign}
            color="bg-green-500"
            trend={permissions.isEmployee ? "Todas las campañas" : "+15% este mes"}
          />
        )}
        {permissions.canViewTasks && (
          <StatCard
            title="Tareas Activas"
            value={stats.tasks}
            icon={CheckSquare}
            color="bg-purple-500"
            trend={permissions.isEmployee ? "Tus tareas asignadas" : null}
          />
        )}
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mt-8">
        <div className="bg-white rounded-lg shadow-md p-6">
          <h2 className="text-xl font-bold text-gray-900 mb-4">Actividad Reciente</h2>
          <div className="space-y-4">
            <div className="flex items-center p-3 bg-gray-50 rounded-lg">
              <div className="w-10 h-10 bg-blue-100 rounded-full flex items-center justify-center">
                <Video className="w-5 h-5 text-blue-600" />
              </div>
              <div className="ml-4 flex-1">
                <p className="text-sm font-medium text-gray-900">Nuevo video publicado</p>
                <p className="text-xs text-gray-500">Hace 2 horas</p>
              </div>
            </div>
            <div className="flex items-center p-3 bg-gray-50 rounded-lg">
              <div className="w-10 h-10 bg-green-100 rounded-full flex items-center justify-center">
                <DollarSign className="w-5 h-5 text-green-600" />
              </div>
              <div className="ml-4 flex-1">
                <p className="text-sm font-medium text-gray-900">Campaña AdSense activada</p>
                <p className="text-xs text-gray-500">Hace 5 horas</p>
              </div>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-lg shadow-md p-6">
          <h2 className="text-xl font-bold text-gray-900 mb-4">Canales Principales</h2>
          <div className="space-y-4">
            <div className="flex items-center justify-between p-3 bg-gray-50 rounded-lg">
              <div className="flex items-center">
                <div className="w-10 h-10 bg-red-100 rounded-full flex items-center justify-center">
                  <Youtube className="w-5 h-5 text-red-600" />
                </div>
                <div className="ml-4">
                  <p className="text-sm font-medium text-gray-900">Canal Principal</p>
                  <p className="text-xs text-gray-500">1.2M suscriptores</p>
                </div>
              </div>
              <span className="text-green-600 text-sm font-medium">Activo</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
