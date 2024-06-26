import Modal from "antd/es/modal/Modal";
import { TaskRequest } from "../services/tasks";
import { useEffect, useState } from "react";
import Input from "antd/es/input/Input";
import TextArea from "antd/es/input/TextArea";
import Select from "antd/es/select";
import { Option } from "antd/es/mentions";
import { format } from 'date-fns';

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
    const [description, setDescription] = useState<string>("");
    const [assignedUserId, setAssignedUserId] = useState<number>(1);
    const [priority, setPriority] = useState<string>("");
    const [status, setStatus] = useState<string>("");
    const [startDate, setStartDate] = useState<string>(format(new Date(), 'yyyy-MM-dd'));
    const [endDate, setEndDate] = useState<string>(format(new Date(), 'yyyy-MM-dd'));

    useEffect(() => {
        setTitle(values.title);
        setDescription(values.description);
        setAssignedUserId(values.assignedUserId);
        setPriority(values.priority);
        setStatus(values.status);
        setStartDate(format(new Date(values.startDate), 'yyyy-MM-dd'));
        setEndDate(format(new Date(values.endDate), 'yyyy-MM-dd'));
    }, [values]);

    const handleOnOk = async () => {
        const taskRequest = {
            title,
            description,
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
                />
                <TextArea
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                    autoSize={{ minRows: 3, maxRows: 3 }}
                    placeholder="Описание"
                />
                <Input
                    value={assignedUserId}
                    onChange={(e) => setAssignedUserId(Number(e.target.value))}
                    placeholder="Id на ком задача"
                />
                <Select
                    value={priority}
                    onChange={(value) => setPriority(value)}
                    placeholder="Приоритет"
                    style={{ width: '100%', marginBottom: '16px' }}
                >
                    <Option value="Низкий">Низкий</Option>
                    <Option value="Средний">Средний</Option>
                    <Option value="Высокий">Высокий</Option>
                </Select>
                <Select
                    value={status}
                    onChange={(value) => setStatus(value)}
                    placeholder="Статус"
                    style={{ width: '100%', marginBottom: '16px' }}
                >
                    <Option value="Не начато">Не начато</Option>
                    <Option value="В процессе">В процессе</Option>
                    <Option value="Завершено">Завершено</Option>
                </Select>
                <Input
                    value={startDate}
                    onChange={(e) => setStartDate(e.target.value)}
                    placeholder="Время начала"
                    type="date"
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
