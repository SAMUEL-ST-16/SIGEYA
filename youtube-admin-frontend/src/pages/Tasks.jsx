import { useState, useEffect } from 'react';
import { CheckSquare, Plus, Edit2, Trash2 } from 'lucide-react';
import taskService from '../services/taskService';
import TaskModal from '../components/modals/TaskModal';
import { usePermissions } from '../hooks/usePermissions';

const Tasks = () => {
  const [tasks, setTasks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [selectedTask, setSelectedTask] = useState(null);
  const permissions = usePermissions();

  useEffect(() => {
    fetchTasks();
  }, []);

  const fetchTasks = async () => {
    try {
      const data = await taskService.getAll();
      setTasks(data);
    } catch (error) {
      console.error('Error:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleCreate = () => {
    setSelectedTask(null);
    setModalOpen(true);
  };

  const handleEdit = (task) => {
    setSelectedTask(task);
    setModalOpen(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('¿Estás seguro de que deseas eliminar esta tarea?')) {
      try {
        await taskService.delete(id);
        fetchTasks();
      } catch (error) {
        console.error('Error deleting task:', error);
        alert('Error al eliminar la tarea');
      }
    }
  };

  const handleSave = async (taskData) => {
    try {
      if (selectedTask) {
        await taskService.update(selectedTask.taskId, taskData);
      } else {
        await taskService.create(taskData);
      }
      setModalOpen(false);
      fetchTasks();
    } catch (error) {
      console.error('Error saving task:', error);
      alert('Error al guardar la tarea');
    }
  };

  const getStatusColor = (status) => {
    const colors = {
      Pending: 'bg-yellow-100 text-yellow-700',
      InProgress: 'bg-blue-100 text-blue-700',
      Completed: 'bg-green-100 text-green-700',
      Cancelled: 'bg-gray-100 text-gray-700',
    };
    return colors[status] || 'bg-gray-100 text-gray-700';
  };

  const getPriorityColor = (priority) => {
    const colors = {
      Low: 'bg-gray-100 text-gray-700',
      Medium: 'bg-blue-100 text-blue-700',
      High: 'bg-orange-100 text-orange-700',
      Urgent: 'bg-red-100 text-red-700',
    };
    return colors[priority] || 'bg-gray-100 text-gray-700';
  };

  if (loading) {
    return <div className="p-8 flex justify-center"><div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div></div>;
  }

  return (
    <div className="p-8">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold text-gray-900">Tareas</h1>
        {permissions.canCreateTask && (
          <button
            onClick={handleCreate}
            className="bg-blue-600 text-white px-4 py-2 rounded-lg flex items-center hover:bg-blue-700"
          >
            <Plus className="w-5 h-5 mr-2" />
            Nueva Tarea
          </button>
        )}
      </div>

      <div className="space-y-4">
        {tasks.map((task) => (
          <div key={task.taskId} className="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition">
            <div className="flex items-start justify-between">
              <div className="flex-1">
                <div className="flex items-center gap-3 mb-2">
                  <h3 className="text-lg font-bold text-gray-900">{task.title}</h3>
                  <span className={`px-3 py-1 text-xs rounded-full ${getStatusColor(task.status)}`}>
                    {task.status}
                  </span>
                  <span className={`px-3 py-1 text-xs rounded-full ${getPriorityColor(task.priority)}`}>
                    {task.priority}
                  </span>
                </div>

                {task.description && (
                  <p className="text-sm text-gray-600 mb-3">{task.description}</p>
                )}

                <div className="flex items-center gap-6 text-sm text-gray-500">
                  <div>
                    <span className="font-medium">Creado por:</span> {task.createdByFullName}
                  </div>
                  {task.assignedToEmployeeName && (
                    <div>
                      <span className="font-medium">Asignado a:</span> {task.assignedToEmployeeName}
                    </div>
                  )}
                  {task.dueDate && (
                    <div>
                      <span className="font-medium">Vence:</span> {new Date(task.dueDate).toLocaleDateString()}
                    </div>
                  )}
                </div>
              </div>

              <div className="flex items-center gap-2">
                {task.isOverdue && (
                  <span className="bg-red-100 text-red-700 px-3 py-1 rounded-full text-xs font-medium">
                    Vencida
                  </span>
                )}
                {permissions.canEditTask && (
                  <button
                    onClick={() => handleEdit(task)}
                    className="text-blue-600 hover:text-blue-900"
                    title="Editar"
                  >
                    <Edit2 className="w-5 h-5" />
                  </button>
                )}
                {permissions.canDeleteTask && (
                  <button
                    onClick={() => handleDelete(task.taskId)}
                    className="text-red-600 hover:text-red-900"
                    title="Eliminar"
                  >
                    <Trash2 className="w-5 h-5" />
                  </button>
                )}
              </div>
            </div>
          </div>
        ))}
      </div>

      {tasks.length === 0 && (
        <div className="text-center py-12">
          <CheckSquare className="w-16 h-16 text-gray-300 mx-auto mb-4" />
          <p className="text-gray-500">No hay tareas registradas</p>
        </div>
      )}

      <TaskModal
        isOpen={modalOpen}
        onClose={() => setModalOpen(false)}
        onSave={handleSave}
        task={selectedTask}
      />
    </div>
  );
};

export default Tasks;
