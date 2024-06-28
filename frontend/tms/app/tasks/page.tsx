"use client";
import Button from "antd/es/button/button";
import { Tasks } from "../components/Tasks";
import { useEffect, useState } from "react";
import {
    TaskRequest,
    changeStateTask,
    createTask,
    deleteTask,
    getAllTasks,
    updateTask
} from "../services/tasks";
import Title from "antd/es/typography/Title";
import { CreateUpdateTask, Mode } from "../components/CreateUpdateTask";
import { message, Modal } from "antd";
import { Role } from "../enums/Role";
import { getUserRole } from "../services/login";

export interface User {
    userId: string;
    name: string;
}

export default function TasksPage() {
    const defaultValues = {
        title: "",
        comment: "",
        assignedUserId: "",
        priority: "",
        status: 1,
        startDate: new Date(),
        endDate: new Date()
    } as Task;

    const [values, setValues] = useState<Task>(defaultValues);
    const [tasks, setTasks] = useState<Task[]>([]);
    const [loading, setLoading] = useState(true);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
    const [taskToDelete, setTaskToDelete] = useState<string | null>(null);
    const [mode, setMode] = useState(Mode.Create);
    const [userRole, setUserRole] = useState<Role>(1);

    useEffect(() => {
        const getTasks = async () => {
            try {
                const tasks = await getAllTasks();
                setTasks(tasks);
                setLoading(false);
                const role = await getUserRole();
                setUserRole(role);
            } catch (error) {
                message.error(error.message);
                setLoading(false);
            }
        };

        getTasks();
    }, []);

    const handleCreateTask = async (request: TaskRequest) => {
        try {
            await createTask(request);
            closeModal();
            const tasks = await getAllTasks();
            setTasks(tasks);
            message.success(`Задача ${request.title} была создана`);
        } catch (error) {
            message.error(error.message);
        }
    };

    const handleUpdateTask = async (id: string, request: TaskRequest) => {
        try {
            await updateTask(id, request);
            closeModal();
            const tasks = await getAllTasks();
            setTasks(tasks);
            message.success(`Задача ${request.title} была обновлена`);
        } catch (error) {
            message.error(error.message);
        }
    };

    const handleAcceptTask = async (id: string) => {
        try {

            await changeStateTask(id);
            const tasks = await getAllTasks();
            setTasks(tasks);
            message.success(`Статус был изменён`);
        } catch (error) {
            message.error(error.message);
        }
    };

    const handleDeleteTask = async (id: string) => {
        setTaskToDelete(id);
        setIsDeleteModalOpen(true);
    };

    const confirmDeleteTask = async () => {
        if (!taskToDelete) return;

        try {
            await deleteTask(taskToDelete);
            message.success("Задача была удалена");
            setIsDeleteModalOpen(false);
            setTaskToDelete(null);
            const tasks = await getAllTasks();
            setTasks(tasks);
        } catch (error) {
            message.error(error.message);
        }
    };

    const cancelDeleteTask = () => {
        setIsDeleteModalOpen(false);
        setTaskToDelete(null);
    };

    const openEditModal = (task: Task) => {
        setMode(Mode.Edit);
        setValues(task);
        setIsModalOpen(true);
    };

    const openModal = () => {
        setMode(Mode.Create);
        setIsModalOpen(true);
    };

    const closeModal = () => {
        setValues(defaultValues);
        setIsModalOpen(false);
    };

    return (
        <div>
            {userRole != Role.Performer && (
                <Button
                    type="primary"
                    style={{ marginTop: "30px" }}
                    size="large"
                    onClick={openModal}
                >
                    Добавить задачу
                </Button>
            )}
            
            <CreateUpdateTask
                mode={mode}
                values={values}
                isModalOpen={isModalOpen}
                handleCreate={handleCreateTask}
                handleUpdate={handleUpdateTask}
                handleCancel={closeModal}
            />

            {loading ? (
                <Title>Loading...</Title>
            ) : (
                <Tasks
                    tasks={tasks}
                    handleOpen={openEditModal}
                    handleDelete={handleDeleteTask}
                    handleAccept={handleAcceptTask}
                    userRole={userRole}
                />
            )}

            <Modal
                title="Подтверждение удаления"
                visible={isDeleteModalOpen}
                onOk={confirmDeleteTask}
                onCancel={cancelDeleteTask}
                okText="Удалить"
                cancelText="Отмена"
            >
                <p>Вы уверены, что хотите удалить эту задачу?</p>
            </Modal>
        </div>
    );
};
