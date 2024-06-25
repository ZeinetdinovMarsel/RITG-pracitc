import Modal from "antd/es/modal/Modal";
import { TaskRequest } from "../services/tasks";
import { useEffect, useState } from "react";
import Input from "antd/es/input/Input"
import TextArea from "antd/es/input/TextArea"

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
    const [startDate, setStartDate] = useState<Date>(new Date);
    const [endDate, setEndDate] = useState<Date>(new Date);

    useEffect(() => {
        setTitle(values.title)
        setDescription(values.description)
        setAssignedUserId(values.assignedUserId)
        setPriority(values.priority)
        setStatus(values.status)
        setStartDate(values.startDate)
        setEndDate(values.endDate)
    }, [values])


    const handleOnOk = async () => {
        const taskRequest = {
            title,
            description,
            assignedUserId,
            priority,
            status,
            startDate,
            endDate
        };

        mode == Mode.Create ? handleCreate(taskRequest) : handleUpdate(values.id, taskRequest);
    };
    return (
        < Modal
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
                <Input
                    value={priority}
                    onChange={(e) => setPriority(e.target.value)}
                    placeholder="Приоритет"
                />
                <Input
                    value={status}
                    onChange={(e) => setStatus(e.target.value)}
                    placeholder="Статус"
                />
                <Input
                    value={startDate}
                    onChange={(e) => setStartDate(e.target.value)}
                    placeholder="Время начала"
                />
                <Input
                    value={endDate}
                    onChange={(e) => setEndDate(e.target.value)}
                    placeholder="Время конца"
                />
            </div>
        </Modal >
    );
};