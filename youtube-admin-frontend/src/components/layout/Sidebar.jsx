import { Link, useLocation } from 'react-router-dom';
import {
  LayoutDashboard,
  Youtube,
  Video,
  DollarSign,
  CheckSquare,
  LogOut,
  User,
  Users
} from 'lucide-react';
import { useAuth } from '../../contexts/AuthContext';
import { usePermissions } from '../../hooks/usePermissions';

const Sidebar = () => {
  const location = useLocation();
  const { logout, user } = useAuth();
  const permissions = usePermissions();

  // Menú completo con permisos
  const allMenuItems = [
    { path: '/dashboard', icon: LayoutDashboard, label: 'Dashboard', show: true },
    // Canales: Admin, Partner, ContentManager y Viewer pueden ver
    { path: '/channels', icon: Youtube, label: 'Canales', show: permissions.canViewChannels },
    // Videos: Admin, Partner, ContentManager y Viewer pueden ver
    { path: '/videos', icon: Video, label: 'Videos', show: permissions.canViewVideos },
    // Campañas: Admin, Partner, Employee y Viewer pueden ver
    { path: '/campaigns', icon: DollarSign, label: 'Campañas', show: permissions.canViewCampaigns },
    // Tareas: Admin, Partner y Employee pueden ver
    { path: '/tasks', icon: CheckSquare, label: 'Tareas', show: permissions.canViewTasks },
    // Usuarios: Solo Admin
    { path: '/users', icon: Users, label: 'Usuarios', show: permissions.canViewAllUsers },
  ];

  // Filtrar solo los items que el usuario puede ver
  const menuItems = allMenuItems.filter(item => item.show);

  const isActive = (path) => location.pathname === path;

  // Función para obtener el badge del rol
  const getRoleBadge = () => {
    const roleColors = {
      'Admin': 'bg-red-600',
      'Partner': 'bg-purple-600',
      'ContentManager': 'bg-blue-600',
      'Employee': 'bg-green-600',
      'Viewer': 'bg-gray-600',
    };

    return (
      <span className={`text-xs px-2 py-1 rounded ${roleColors[user?.roleName] || 'bg-gray-600'}`}>
        {user?.roleName}
      </span>
    );
  };

  return (
    <div className="bg-gray-900 text-white w-64 min-h-screen flex flex-col">
      <div className="p-6 border-b border-gray-800">
        <h1 className="text-2xl font-bold">YouTube Manager</h1>
        <div className="mt-3 flex items-center gap-2">
          <User className="w-4 h-4 text-gray-400" />
          <div className="flex-1">
            <p className="text-sm text-gray-300">{user?.username}</p>
            <p className="text-xs text-gray-400">{user?.email}</p>
          </div>
        </div>
        <div className="mt-2">
          {getRoleBadge()}
        </div>
      </div>

      <nav className="flex-1 p-4 space-y-2">
        {menuItems.map((item) => {
          const Icon = item.icon;
          return (
            <Link
              key={item.path}
              to={item.path}
              className={`flex items-center space-x-3 px-4 py-3 rounded-lg transition ${
                isActive(item.path)
                  ? 'bg-blue-600 text-white'
                  : 'text-gray-300 hover:bg-gray-800'
              }`}
            >
              <Icon className="w-5 h-5" />
              <span>{item.label}</span>
            </Link>
          );
        })}
      </nav>

      <div className="p-4 border-t border-gray-800">
        <button
          onClick={logout}
          className="flex items-center space-x-3 px-4 py-3 rounded-lg text-gray-300 hover:bg-gray-800 transition w-full"
        >
          <LogOut className="w-5 h-5" />
          <span>Cerrar Sesión</span>
        </button>
      </div>
    </div>
  );
};

export default Sidebar;
