import Modal from "antd/es/modal/Modal";
import { TaskRequest } from "../services/tasks";
import { useEffect, useState } from "react";
import Input from "antd/es/input/Input";
import TextArea from "antd/es/input/TextArea";
import Select from "antd/es/select";
import { Option } from "antd/es/mentions";
import { format } from 'date-fns';
import { getUsersbyRole } from "../services/tasks";
import { Button } from "antd";
import { Role } from "../enums/Role";

interface Props {
    mode: Mode;
    values: Task;
    isModalOpen: boolean;
    handleCancel: () => void;
    handleCreate: (request: TaskRequest) => void;
    handleUpdate: (id: string, request: TaskRequest) => void;
}

export enum Mode {
    Create,
    Edit,
}

export const CreateUpdateTask = ({
    mode,
    values,
    isModalOpen,
    handleCancel,
    handleCreate,
    handleUpdate,
}: Props) => {
    const [title, setTitle] = useState<string>("");
    const [comment, setComment] = useState<string>("");
    const [assignedUserId, setAssignedUserId] = useState<string>("");
    const [priority, setPriority] = useState<number>(1);
    const [status, setStatus] = useState<number>(1);
    const [startDate, setStartDate] = useState<string>(format(new Date(), 'yyyy-MM-dd'));
    const [endDate, setEndDate] = useState<string>(format(new Date(), 'yyyy-MM-dd'));
    const [users, setUsers] = useState<{ userId: string; name: string }[]>([]);


    useEffect(() => {
        const fetchUsers = async () => {
            try {
                const usersData = await getUsersbyRole(Role.Performer);
                setUsers(usersData);
            } catch (error) {
                console.error("Error fetching users:", error);
            }
        };

        fetchUsers();
        
        setTitle(values.title);
        setComment(values.comment);
        setAssignedUserId(values.assignedUserId);
        setPriority(values.priority);
        setStatus(values.status);
        setStartDate(format(new Date(values.startDate), 'yyyy-MM-dd'));
        setEndDate(format(new Date(values.endDate), 'yyyy-MM-dd'));
    }, [values]);

    const handleOnOk = async () => {
        const taskRequest = {
            title,
            comment,
            assignedUserId,
            priority,
            status,
            startDate: new Date(startDate),
            endDate: new Date(endDate)
        };

        mode === Mode.Create ? handleCreate(taskRequest) : handleUpdate(values.id, taskRequest);
    };

    return (
        <Modal
            title={
                mode === Mode.Create ? "Добавить задачу" : "Редактировать задачу"
            }
            open={isModalOpen}
            cancelText={"Oтмена"}
            onOk={handleOnOk}
            onCancel={handleCancel}
        >
            <div className="book_modal">
                <Input
                    value={title}
                    onChange={(e) => setTitle(e.target.value)}
                    placeholder="Название"
                    style={{ width: '100%', marginBottom: '16px' }}
                />
                <TextArea
                    value={comment}
                    onChange={(e) => setComment(e.target.value)}
                    autoSize={{ minRows: 3, maxRows: 3 }}
                    placeholder="Комментарий"
                    style={{ width: '100%', marginBottom: '16px' }}
                />
                <Select
                    value={assignedUserId}
                    onChange={(value) => setAssignedUserId(value)}
                    placeholder="Id на ком задача"
                    style={{ width: '100%', marginBottom: '16px' }}
                >
                    {users.map((user) => (
                        <Option key={user.userId} value={user.userId}>
                            {user.name}
                        </Option>
                    ))}
                </Select>
                <Select
                    value={priority}
                    onChange={(value) => setPriority(value)}
                    placeholder="Приоритет"
                    style={{ width: '100%', marginBottom: '16px' }}
                >
                    <Option value={1}>Низкий</Option>
                    <Option value={2}>Средний</Option>
                    <Option value={3}>Высокий</Option>
                </Select>
                <Select
                    value={status}
                    onChange={(value) => setStatus(value)}
                    placeholder="Статус"
                    style={{ width: '100%', marginBottom: '16px' }}
                >
                    <Option value={1}>Не принят</Option>
                    <Option value={2}>В разработке</Option>
                    <Option value={3}>Завершена</Option>
                </Select>
                <Input
                    value={startDate}
                    onChange={(e) => setStartDate(e.target.value)}
                    placeholder="Время начала"
                    type="date"
                    style={{ width: '100%', marginBottom: '16px' }}
                />
                <Input
                    value={endDate}
                    onChange={(e) => setEndDate(e.target.value)}
                    placeholder="Время конца"
                    type="date"
                />
            </div>
        </Modal>
    );
};
