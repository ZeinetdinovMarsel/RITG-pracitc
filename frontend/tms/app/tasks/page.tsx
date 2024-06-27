"use client";

import Button from "antd/es/button/button"
import { Tasks } from "../components/Tasks"
import { useEffect, useState } from "react";
import {
    TaskRequest,
    createTask,
    deleteTask,
    getAllTasks,
    updateTask
} from "../services/tasks";
import Title from "antd/es/typography/Title"
import { CreateUpdateTask, Mode } from "../components/CreateUpdateTask";
import { message } from "antd";

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
        status: "Не начато",
        startDate: new Date,
        endDate: new Date
    } as Task

    const [values, setValues] = useState<Task>(defaultValues);
    const [tasks, setTasks] = useState<Task[]>([]);
    const [loading, setLoading] = useState(true);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [mode, setMode] = useState(Mode.Create);
    const [error, setError] = useState<string | null>(null);
    useEffect(() => {
        const getTasks = async () => {
            try {
                const tasks = await getAllTasks();
                setTasks(tasks);
                setLoading(false);
            } catch (error) {
                setError(error.message);
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
    }
    
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
    }
    
    const handleDeleteTask = async (id: string) => {
        try {
            await deleteTask(id);
            closeModal();
            const tasks = await getAllTasks();
            setTasks(tasks);
            message.success(`Задача была удалена`);
        } catch (error) {
            message.error(error.message);
        }
    }
    

    const openEditModal = (task: Task) => {
        setMode(Mode.Edit);
        setValues(task);
        setIsModalOpen(true);
    }

    const openModal = () => {
        setMode(Mode.Create);
        setIsModalOpen(true);
    }

    const closeModal = () => {
        setValues(defaultValues);
        setIsModalOpen(false);
    }

    return (
        <div>
            {error && (
                <div style={{ backgroundColor: 'red', color: 'white', padding: '10px', marginBottom: '10px' }}>
                    {error}
                </div>
            )}

            <Button
                type="primary"
                style={{ marginTop: "30px" }}
                size="large"
                onClick={openModal}
            >
                Добавить задачу
            </Button>
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
                />
            )}
        </div>
    )
}