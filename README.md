# YouTube Content Manager & AdSense Administration Platform

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-blue)
![React](https://img.shields.io/badge/React-19.0-61DAFB)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927)
![License](https://img.shields.io/badge/license-MIT-green)

Una plataforma completa para la gestión de contenido de YouTube, campañas de AdSense, administración de empleados y seguimiento de tareas. Incluye autenticación JWT y OAuth 2.0 (Google), control de acceso basado en roles (RBAC) y arquitectura limpia.

## 📋 Tabla de Contenidos

- [Características](#-características)
- [Tecnologías](#-tecnologías)
- [Arquitectura](#-arquitectura)
- [Requisitos Previos](#-requisitos-previos)
- [Instalación](#-instalación)
- [Configuración](#-configuración)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Roles y Permisos](#-roles-y-permisos)
- [API Endpoints](#-api-endpoints)
- [Base de Datos](#-base-de-datos)
- [Despliegue](#-despliegue)
- [Autor](#-autor)

## ✨ Características

### Backend (API REST)
- 🔐 **Autenticación Dual**: JWT y OAuth 2.0 (Google)
- 👥 **Control de Acceso Basado en Roles (RBAC)**: Admin, Partner, ContentManager, Employee, Viewer
- 📺 **Gestión de Canales de YouTube**: CRUD completo con métricas
- 🎥 **Administración de Videos**: Seguimiento de estadísticas y categorías
- 💰 **Campañas de AdSense**: Gestión de presupuestos y rendimiento
- 📊 **Analytics de Videos**: Vistas, likes, comentarios, tiempo de visualización
- ✅ **Sistema de Tareas**: Asignación, seguimiento y comentarios
- 👤 **Gestión de Usuarios**: CRUD con validación y hash de contraseñas
- 📝 **Logging con Serilog**: Registro estructurado de eventos
- ✅ **Validación con FluentValidation**: Validaciones robustas en DTOs
- 🗺️ **AutoMapper**: Mapeo automático de entidades a DTOs
- 📚 **Documentación Swagger**: API documentada y explorable

### Frontend (React SPA)
- ⚡ **Vite**: Build rápido y HMR
- 🎨 **Tailwind CSS**: Diseño moderno y responsive
- 🔄 **React Router**: Navegación declarativa
- 🔐 **Protected Routes**: Rutas protegidas por autenticación
- 📱 **Responsive Design**: Compatible con todos los dispositivos
- 🌐 **Google OAuth Integration**: Login con Google
- 📊 **Dashboard Interactivo**: Visualización de métricas clave
- 🎯 **Context API**: Gestión de estado global

## 🛠️ Tecnologías

### Backend
- **Framework**: ASP.NET Core 8.0
- **ORM**: Entity Framework Core 8.0
- **Base de Datos**: SQL Server 2022
- **Autenticación**: JWT Bearer + Google OAuth 2.0
- **Logging**: Serilog
- **Validación**: FluentValidation
- **Mapeo**: AutoMapper
- **Documentación**: Swashbuckle (Swagger/OpenAPI)
- **Seguridad**: BCrypt.Net para hashing de contraseñas

### Frontend
- **Framework**: React 19.0
- **Build Tool**: Vite 7.2
- **Routing**: React Router DOM 7.1
- **Styling**: Tailwind CSS 3.4
- **HTTP Client**: Axios 1.7
- **OAuth**: @react-oauth/google 0.12
- **Icons**: lucide-react 0.468

## 🏗️ Arquitectura

```
┌─────────────────────────────────────────────────────────────┐
│                         Frontend (React)                     │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │   Pages      │  │  Components  │  │   Services   │      │
│  │              │  │              │  │              │      │
│  │ - Login      │  │ - Layout     │  │ - authService│      │
│  │ - Dashboard  │  │ - Navbar     │  │ - api        │      │
│  │ - Videos     │  │ - Tables     │  │              │      │
│  │ - Channels   │  │ - Forms      │  │              │      │
│  │ - Tasks      │  │              │  │              │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
│                           ↓                                  │
│                    Context API (Auth)                        │
└─────────────────────────────────────────────────────────────┘
                             ↓ HTTPS/REST
┌─────────────────────────────────────────────────────────────┐
│                    Backend (ASP.NET Core)                    │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │ Controllers  │  │   Services   │  │ Repositories │      │
│  │              │  │              │  │              │      │
│  │ - Auth       │  │ - AuthService│  │ - Generic    │      │
│  │ - Videos     │  │ - VideoServ. │  │ - Video      │      │
│  │ - Channels   │  │ - ChannelS.  │  │ - Task       │      │
│  │ - Tasks      │  │ - TaskService│  │ - AdSense    │      │
│  │ - Users      │  │ - OAuthServ. │  │              │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
│         ↓                  ↓                  ↓              │
│  ┌──────────────────────────────────────────────────────┐  │
│  │              Entity Framework Core (ORM)              │  │
│  └──────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                             ↓
┌─────────────────────────────────────────────────────────────┐
│                      SQL Server Database                     │
│  - Users          - YouTubeChannels    - Videos             │
│  - Roles          - VideoCategories    - VideoAnalytics     │
│  - Tasks          - TaskComments       - AdSenseCampaigns   │
│  - AdRevenues                                               │
└─────────────────────────────────────────────────────────────┘
```

## 📦 Requisitos Previos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)
- [SQL Server 2022](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) o SQL Server Express
- [Git](https://git-scm.com/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o [Visual Studio Code](https://code.visualstudio.com/)
- Cuenta de Google Cloud (para OAuth)

## 🚀 Instalación

### 1. Clonar el Repositorio

```bash
git clone https://github.com/tu-usuario/ProjectFinally.git
cd ProjectFinally
```

### 2. Configurar Backend

```bash
cd ProjectFinally

# Restaurar paquetes NuGet
dotnet restore

# Actualizar la cadena de conexión en appsettings.json
# (Ver sección de Configuración)

# Aplicar migraciones a la base de datos
dotnet ef database update

# Ejecutar el backend
dotnet run --launch-profile https
```

El backend estará disponible en:
- HTTPS: `https://localhost:7273`
- HTTP: `http://localhost:5091`
- Swagger UI: `https://localhost:7273` (en modo Development)

### 3. Configurar Frontend

```bash
cd youtube-admin-frontend

# Instalar dependencias
npm install

# Crear archivo .env con las variables de entorno
# (Ver sección de Configuración)

# Ejecutar el frontend
npm run dev
```

El frontend estará disponible en: `http://localhost:5180`

## ⚙️ Configuración

### Backend - appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=YouTubeContentDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "tu-clave-secreta-super-segura-de-al-menos-32-caracteres",
    "Issuer": "YouTubeContentAPI",
    "Audience": "YouTubeContentClient",
    "ExpirationMinutes": 60
  },
  "OAuth": {
    "Google": {
      "ClientId": "TU_GOOGLE_CLIENT_ID.apps.googleusercontent.com"
    },
    "Facebook": {
      "AppId": "TU_FACEBOOK_APP_ID",
      "AppSecret": "TU_FACEBOOK_APP_SECRET"
    }
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/log-.txt",
          "rollingInterval": "Day"
        }
      }
    ]
  }
}
```

### Frontend - .env

```env
VITE_GOOGLE_CLIENT_ID=TU_GOOGLE_CLIENT_ID.apps.googleusercontent.com
VITE_FACEBOOK_APP_ID=TU_FACEBOOK_APP_ID
VITE_API_URL=https://localhost:7273/api
```

### Configurar Google OAuth 2.0

1. Ve a [Google Cloud Console](https://console.cloud.google.com/)
2. Crea un nuevo proyecto o selecciona uno existente
3. Navega a **APIs & Services > Credentials**
4. Crea **OAuth 2.0 Client ID** (tipo: Web application)
5. Configura **Authorized JavaScript origins**:
   ```
   http://localhost:3000
   http://localhost:5173
   http://localhost:5176
   http://localhost:5178
   http://localhost:5179
   http://localhost:5180
   ```
6. Copia el **Client ID** y úsalo en `appsettings.json` y `.env`

### Usuarios por Defecto (Seeder)

El sistema incluye usuarios de prueba para cada rol:

| Username | Password | Email | Rol |
|----------|----------|-------|-----|
| admin | Admin123! | admin@youtubemanager.com | Admin |
| partner1 | Partner123! | partner@youtubemanager.com | Partner |
| contentmanager1 | Content123! | content@youtubemanager.com | ContentManager |
| employee1 | Employee123! | employee@youtubemanager.com | Employee |
| viewer1 | Viewer123! | viewer@youtubemanager.com | Viewer |

## 📁 Estructura del Proyecto

```
ProjectFinally/
├── ProjectFinally/                 # Backend (API)
│   ├── Controllers/               # Controladores REST API
│   │   ├── AuthController.cs
│   │   ├── VideosController.cs
│   │   ├── YouTubeChannelsController.cs
│   │   ├── AdSenseCampaignsController.cs
│   │   ├── TasksController.cs
│   │   └── UsersController.cs
│   ├── Services/                  # Lógica de negocio
│   │   ├── Interfaces/
│   │   └── Implementations/
│   ├── Repositories/              # Acceso a datos
│   │   ├── Interfaces/
│   │   └── Implementations/
│   ├── Models/                    # Entidades y DTOs
│   │   ├── Entities/
│   │   └── DTOs/
│   ├── Data/                      # DbContext y Seeders
│   │   ├── ApplicationDbContext.cs
│   │   └── Seeders/
│   ├── Helpers/                   # Utilidades
│   │   ├── JwtHelper.cs
│   │   └── PasswordHasher.cs
│   ├── Mappings/                  # Perfiles de AutoMapper
│   ├── Validators/                # Validadores FluentValidation
│   ├── Migrations/                # Migraciones de EF Core
│   ├── appsettings.json          # Configuración
│   └── Program.cs                # Punto de entrada
│
├── youtube-admin-frontend/        # Frontend (React)
│   ├── src/
│   │   ├── pages/                # Páginas/Vistas
│   │   │   ├── Login.jsx
│   │   │   ├── Register.jsx
│   │   │   ├── Dashboard.jsx
│   │   │   ├── Videos.jsx
│   │   │   ├── Channels.jsx
│   │   │   ├── Campaigns.jsx
│   │   │   ├── Tasks.jsx
│   │   │   └── Users.jsx
│   │   ├── components/           # Componentes reutilizables
│   │   │   ├── layout/
│   │   │   └── common/
│   │   ├── contexts/             # Context API
│   │   │   └── AuthContext.jsx
│   │   ├── services/             # Servicios HTTP
│   │   │   ├── api.js
│   │   │   └── authService.js
│   │   ├── App.jsx
│   │   └── main.jsx
│   ├── .env                      # Variables de entorno
│   ├── package.json
│   ├── vite.config.js
│   └── tailwind.config.js
│
└── README.md                      # Este archivo
```

## 👥 Roles y Permisos

### Roles del Sistema

| Rol | Descripción | Permisos |
|-----|-------------|----------|
| **Admin** | Administrador del sistema | Acceso completo a todas las funcionalidades |
| **Partner** | Socio de negocio | Gestión de canales propios, campañas y reportes |
| **ContentManager** | Gestor de contenido | Administración de videos, canales y categorías |
| **Employee** | Empleado | Gestión de tareas asignadas y comentarios |
| **Viewer** | Visor | Solo lectura de contenido público |

### Matriz de Permisos

| Funcionalidad | Admin | Partner | ContentManager | Employee | Viewer |
|---------------|-------|---------|----------------|----------|--------|
| Gestión de Usuarios | ✅ | ❌ | ❌ | ❌ | ❌ |
| Gestión de Roles | ✅ | ❌ | ❌ | ❌ | ❌ |
| Campañas AdSense | ✅ | ✅ | ✅ | ❌ | 👁️ |
| Canales YouTube | ✅ | ✅ | ✅ | ❌ | 👁️ |
| Videos | ✅ | ✅ | ✅ | ❌ | 👁️ |
| Tareas | ✅ | ✅ | ✅ | ✅ | ❌ |
| Analytics | ✅ | ✅ | ✅ | 👁️ | 👁️ |

*✅ = Control total | 👁️ = Solo lectura*

## 🔌 API Endpoints

### Autenticación

```http
POST   /api/auth/login              # Login con credenciales
POST   /api/auth/register           # Registro de usuario
POST   /api/auth/oauth-login        # Login con OAuth (Google)
```

### Usuarios

```http
GET    /api/users                   # Listar usuarios
GET    /api/users/{id}              # Obtener usuario por ID
POST   /api/users                   # Crear usuario
PUT    /api/users/{id}              # Actualizar usuario
DELETE /api/users/{id}              # Eliminar usuario
```

### Canales de YouTube

```http
GET    /api/youtubechannels         # Listar canales
GET    /api/youtubechannels/{id}    # Obtener canal por ID
POST   /api/youtubechannels         # Crear canal
PUT    /api/youtubechannels/{id}    # Actualizar canal
DELETE /api/youtubechannels/{id}    # Eliminar canal
```

### Videos

```http
GET    /api/videos                  # Listar videos
GET    /api/videos/{id}             # Obtener video por ID
POST   /api/videos                  # Crear video
PUT    /api/videos/{id}             # Actualizar video
DELETE /api/videos/{id}             # Eliminar video
GET    /api/videos/{id}/analytics   # Obtener analytics del video
```

### Campañas AdSense

```http
GET    /api/adsensecampaigns        # Listar campañas
GET    /api/adsensecampaigns/{id}   # Obtener campaña por ID
POST   /api/adsensecampaigns        # Crear campaña
PUT    /api/adsensecampaigns/{id}   # Actualizar campaña
DELETE /api/adsensecampaigns/{id}   # Eliminar campaña
```

### Tareas

```http
GET    /api/tasks                   # Listar tareas
GET    /api/tasks/{id}              # Obtener tarea por ID
POST   /api/tasks                   # Crear tarea
PUT    /api/tasks/{id}              # Actualizar tarea
DELETE /api/tasks/{id}              # Eliminar tarea
GET    /api/tasks/{id}/comments     # Obtener comentarios de tarea
POST   /api/tasks/{id}/comments     # Agregar comentario a tarea
```

### Documentación Swagger

Accede a la documentación interactiva en: `https://localhost:7273` (solo en Development)

## 💾 Base de Datos

### Modelo de Datos (ERD Simplificado)

```
┌──────────────┐         ┌──────────────────┐
│    Roles     │────<────│      Users       │
└──────────────┘         └──────────────────┘
                                  │
                                  │ (OwnerId)
                                  ▼
┌──────────────────┐    ┌──────────────────┐
│ YouTubeChannels  │───<│     Videos       │
└──────────────────┘    └──────────────────┘
        │                        │
        │                        │
        ▼                        ▼
┌──────────────────┐    ┌──────────────────┐
│AdSenseCampaigns  │    │ VideoAnalytics   │
└──────────────────┘    └──────────────────┘
        │
        ▼
┌──────────────────┐
│   AdRevenues     │
└──────────────────┘

┌──────────────────┐    ┌──────────────────┐
│      Tasks       │───<│  TaskComments    │
└──────────────────┘    └──────────────────┘
```

### Migraciones

```bash
# Crear nueva migración
dotnet ef migrations add NombreMigracion

# Aplicar migraciones
dotnet ef database update

# Revertir última migración
dotnet ef database update NombreMigracionAnterior

# Eliminar última migración
dotnet ef migrations remove
```

## 🌐 Despliegue

### Backend (Azure App Service / IIS)

1. **Publicar la aplicación**:
   ```bash
   dotnet publish -c Release -o ./publish
   ```

2. **Configurar variables de entorno**:
   - Connection String
   - JWT Secret
   - OAuth Client IDs

3. **Configurar CORS** para el dominio de producción en `Program.cs`

4. **Aplicar migraciones** en producción:
   ```bash
   dotnet ef database update --connection "tu-connection-string"
   ```

### Frontend (Vercel / Netlify / Azure Static Web Apps)

1. **Configurar variables de entorno** en la plataforma:
   ```
   VITE_API_URL=https://tu-api.azurewebsites.net/api
   VITE_GOOGLE_CLIENT_ID=tu-client-id
   ```

2. **Build del proyecto**:
   ```bash
   npm run build
   ```

3. **Desplegar** la carpeta `dist/`

### Variables de Entorno Producción

#### Backend
```bash
ConnectionStrings__DefaultConnection="Server=..."
Jwt__Key="tu-clave-segura-produccion"
OAuth__Google__ClientId="tu-client-id-produccion"
ASPNETCORE_ENVIRONMENT=Production
```

#### Frontend
```bash
VITE_API_URL=https://api.tudominio.com/api
VITE_GOOGLE_CLIENT_ID=tu-client-id-produccion
```

### Configurar Google OAuth para Producción

En Google Cloud Console, agrega los dominios de producción:
```
https://tudominio.com
https://www.tudominio.com
```

## 🔒 Seguridad

- ✅ Contraseñas hasheadas con BCrypt
- ✅ Tokens JWT con expiración configurable
- ✅ CORS configurado y restrictivo
- ✅ HTTPS enforced en producción
- ✅ Validación de datos con FluentValidation
- ✅ SQL Injection protegido (Entity Framework)
- ✅ XSS protegido (React escape automático)
- ✅ OAuth 2.0 para autenticación de terceros

## 📝 Scripts Disponibles

### Backend
```bash
dotnet run                    # Ejecutar en modo desarrollo
dotnet watch run              # Ejecutar con hot reload
dotnet test                   # Ejecutar tests
dotnet build                  # Compilar proyecto
dotnet publish -c Release     # Compilar para producción
```

### Frontend
```bash
npm run dev                   # Servidor de desarrollo
npm run build                 # Build para producción
npm run preview               # Preview del build
npm run lint                  # Linter
```

## 🧪 Testing

### Backend
```bash
# Ejecutar todos los tests
dotnet test

# Ejecutar tests con coverage
dotnet test /p:CollectCoverage=true
```

### Frontend
```bash
# Ejecutar tests (cuando estén configurados)
npm test
```

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo [LICENSE](LICENSE) para más detalles.

## 👤 Autor

**Samuel Soto Trujillo**

- Email: antonhy1608@gmail.com
- GitHub: [@tu-usuario](https://github.com/tu-usuario)

---

## 🙏 Agradecimientos

- [Microsoft Docs](https://docs.microsoft.com/)
- [React Documentation](https://react.dev/)
- [Tailwind CSS](https://tailwindcss.com/)
- Comunidad de desarrolladores

---

**Desarrollado con ❤️ por Samuel Soto Trujillo**

*Última actualización: Diciembre 2025*
