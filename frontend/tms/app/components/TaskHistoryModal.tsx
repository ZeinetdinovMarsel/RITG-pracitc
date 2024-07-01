import { Modal, Table } from "antd";
import React, { useEffect, useState } from "react";
import { getUsersbyRole } from "../services/tasks";
import { Role } from "../enums/Role";

interface TaskHistoryModalProps {
    visible: boolean;
    onClose: () => void;
    taskHistory: any[];
}

const TaskHistoryModal: React.FC<TaskHistoryModalProps> = ({
    visible,
    onClose,
    taskHistory,
}) => {
    const [users, setUsers] = useState<{ userId: string; name: string }[]>([]);

    const statusLabels: { [key: number]: string } = {
        1: "Не принят",
        2: "Принят",
        3: "Завершён",
    };

    const priorityLabels: { [key: number]: string } = {
        1: "Низкий",
        2: "Средний",
        3: "Высокий",
    };

    useEffect(() => {
        const getUsers = async () => {
            try {
                const usersDataPerformer = await getUsersbyRole(Role.Performer);
                const usersDataManager = await getUsersbyRole(Role.Manager);
                const usersDataAdmin = await getUsersbyRole(Role.Admin);
                
                const usersData = [
                    ...usersDataPerformer,
                    ...usersDataManager,
                    ...usersDataAdmin,
                ];
                setUsers(usersData);
            } catch (error) {
                console.error("Неизвестная ошибка: ", error);
            }
        };

        getUsers();
    }, []);

    const findUserNameById = (userId: string) => {
        const user = users.find((user) => user.userId === userId);
        return user ? user.name : "Неизвестный пользователь";
    };

    const parseChanges = (changes: string) => {
        const changeItems = changes.split(", ").map((change) => {
            if (change.includes("Статус:")) {
                return change.replace(/Статус: (\d) -> (\d)/, (_, from, to) =>
                    `Статус: ${statusLabels[Number(from)]} -> ${statusLabels[Number(to)]}`
                );
            } else if (change.includes("Приоритет:")) {
                return change.replace(/Приоритет: (\d) -> (\d)/, (_, from, to) =>
                    `Приоритет: ${priorityLabels[Number(from)]} -> ${priorityLabels[Number(to)]}`
                );
            }
            return change;
        });
        return changeItems;
    };

    const columns = [
        {
            title: "Дата изменения",
            dataIndex: "changeDate",
            key: "changeDate",
            render: (changeDate: string) => new Date(changeDate).toLocaleString(),
        },
        {
            title: "Изменил",
            dataIndex: "userId",
            key: "userId",
            render: (userId: string) => findUserNameById(userId),
        },
        {
            title: "Изменения",
            dataIndex: "changes",
            key: "changes",
            render: (changes: string) => {
                const firstColonIndex = changes.indexOf(":") + 1;

                let title = changes;
                let changeItems = [];

                if (firstColonIndex !== -1) {
                    title = changes.substring(0, firstColonIndex);
                    const rest = changes.substring(firstColonIndex);

                    changeItems = parseChanges(rest);
                }

                return (
                    <div>
                        <strong>{title}</strong>
                        <ul>
                            {changeItems.map((item, index) => (
                                <li key={index}>{item}</li>
                            ))}
                        </ul>
                    </div>
                );
            },
        },
    ];

    return (
        <Modal
            title="История изменений задачи"
            visible={visible}
            onCancel={onClose}
            footer={null}
        >
            <Table
                columns={columns}
                dataSource={taskHistory}
                pagination={false}
                rowKey={(record) => record.id}
            />
        </Modal>
    );
};

export default TaskHistoryModal;