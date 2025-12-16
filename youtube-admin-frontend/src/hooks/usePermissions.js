import { useAuth } from '../contexts/AuthContext';

const usePermissions = () => {
  const { user } = useAuth();

  const roleName = user?.roleName;

  const permissions = {
    // ========== CHANNELS ==========
    // Admin: puede ver y gestionar todos los canales (+ filtro por usuario)
    // Partner: puede ver y gestionar todos los canales
    // ContentManager: puede crear múltiples canales y gestionar solo SUS PROPIOS canales
    // Employee: NO tiene acceso a canales
    // Viewer: solo puede VER todos los canales (no editar)
    canViewAllChannels: roleName === 'Admin' || roleName === 'Partner' || roleName === 'Viewer' || roleName === 'ContentManager',
    canCreateChannel: roleName === 'Admin' || roleName === 'Partner' || roleName === 'ContentManager',
    canEditChannel: roleName === 'Admin' || roleName === 'Partner',
    canDeleteChannel: roleName === 'Admin' || roleName === 'Partner',
    canEditOwnChannel: roleName === 'ContentManager',
    canDeleteOwnChannel: roleName === 'ContentManager',
    canViewChannels: roleName !== 'Employee', // Todos excepto Employee

    // ========== VIDEOS ==========
    // Admin: puede ver y gestionar todos los videos (+ filtro por usuario)
    // Partner: puede ver y gestionar todos los videos
    // ContentManager: puede crear múltiples videos y gestionar solo SUS PROPIOS videos
    // Employee: NO tiene acceso a videos
    // Viewer: solo puede VER todos los videos (no editar)
    canViewAllVideos: roleName === 'Admin' || roleName === 'Partner' || roleName === 'Viewer' || roleName === 'ContentManager',
    canCreateVideo: roleName === 'Admin' || roleName === 'Partner' || roleName === 'ContentManager',
    canEditVideo: roleName === 'Admin' || roleName === 'Partner',
    canDeleteVideo: roleName === 'Admin' || roleName === 'Partner',
    canEditOwnVideo: roleName === 'ContentManager',
    canDeleteOwnVideo: roleName === 'ContentManager',
    canViewVideos: roleName !== 'Employee', // Todos excepto Employee

    // ========== CAMPAIGNS (AdSense) ==========
    // Admin: puede ver y gestionar todas las campañas (+ filtro por usuario)
    // Partner: puede ver y gestionar todas las campañas
    // Employee: puede ver y gestionar TODAS las campañas (gestión de operaciones)
    // Viewer: puede VER todas las campañas (para ver dinero y ganancias)
    // ContentManager: NO puede ver ni gestionar campañas
    canViewAllCampaigns: roleName === 'Admin' || roleName === 'Partner' || roleName === 'Employee' || roleName === 'Viewer',
    canCreateCampaign: roleName === 'Admin' || roleName === 'Partner' || roleName === 'Employee',
    canEditCampaign: roleName === 'Admin' || roleName === 'Partner' || roleName === 'Employee',
    canDeleteCampaign: roleName === 'Admin' || roleName === 'Partner' || roleName === 'Employee',
    canEditOwnCampaign: false, // Ya no aplica - Employee edita TODAS
    canViewCampaigns: roleName !== 'ContentManager', // Todos excepto ContentManager

    // ========== TASKS (Tareas) ==========
    // Admin: puede ver y gestionar todas las tareas (+ filtro por usuario)
    // Partner: puede ver y gestionar todas las tareas
    // Employee: puede ver y gestionar SUS TAREAS ASIGNADAS
    // Viewer: puede VER todas las tareas
    // ContentManager: NO puede ver ni gestionar tareas
    canViewAllTasks: roleName === 'Admin' || roleName === 'Partner' || roleName === 'Viewer' || roleName === 'Employee',
    canCreateTask: roleName === 'Admin' || roleName === 'Partner' || roleName === 'Employee',
    canEditTask: roleName === 'Admin' || roleName === 'Partner' || roleName === 'Employee',
    canDeleteTask: roleName === 'Admin' || roleName === 'Partner' || roleName === 'Employee',
    canEditOwnTask: false, // Ya no aplica - se usa canEditTask directamente
    canViewTasks: roleName !== 'ContentManager', // Todos excepto ContentManager

    // ========== USERS (Gestión de usuarios) ==========
    // Solo Admin puede gestionar usuarios
    // Partner NO puede ver ni gestionar usuarios
    canViewAllUsers: roleName === 'Admin',
    canManageUsers: roleName === 'Admin',
    canCreateUser: roleName === 'Admin',
    canEditUser: roleName === 'Admin',
    canDeleteUser: roleName === 'Admin',

    // ========== ROLES ==========
    isAdmin: roleName === 'Admin',
    isPartner: roleName === 'Partner',
    isContentManager: roleName === 'ContentManager',
    isEmployee: roleName === 'Employee',
    isViewer: roleName === 'Viewer',
  };

  return permissions;
};

export { usePermissions };
