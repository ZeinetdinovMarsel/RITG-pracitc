import Card from "antd/es/card/Card";
import { CardTitle } from "./Cardtitle";
import Button from "antd/es/button/button";
import { useEffect, useState } from "react";
import { getUsersbyRole } from "../services/tasks";
import { Role } from "../enums/Role";

interface Props {
    tasks: Task[];
    handleDelete: (id: string) => void;
    handleOpen: (task: Task) => void;
    handleAccept: (id: string) => void;
    userRole: Role;
    showHistory: (id: string) => void;
}

const sortTasks = (tasks: Task[]) => {
    return tasks.sort((a, b) => {
        if (a.status !== b.status) {
            return a.status - b.status;
        }
        const priorityComparison = b.priority - a.priority;
        if (priorityComparison !== 0) {
            return priorityComparison;
        }

        return new Date(a.endDate).getTime() - new Date(b.endDate).getTime();
    });
};

export const Tasks = ({ tasks, handleDelete, handleOpen, handleAccept, userRole, showHistory }: Props) => {
    const sortedTasks = sortTasks(tasks);
    const [users, setUsers] = useState<{ userId: string; name: string }[]>([]);

    useEffect(() => {
        const getUsers = async () => {
            try {
                const usersDataPerformer = await getUsersbyRole(Role.Performer);
                const usersDataManager = await getUsersbyRole(Role.Manager);
                const usersData = [...usersDataPerformer, ...usersDataManager];
                setUsers(usersData);
            } catch (error) {
                console.error("Неизвестная ошибка:", error);
            }
        };

        getUsers();
    }, []);

    const getAssignedUserName = (userId: string) => {
        const user = users.find((user) => user.userId === userId);
        return user ? user.name : "Неизвестный исполнитель";
    };

    const handleAcceptTask = async (id: string) => {
        handleAccept(id);
    };

    return (
        <div className="cards">
            {sortedTasks.map((task: Task) => (
                !(userRole === Role.Performer && task.status === 3) && (
                    <Card
                        key={task.id}
                        title={<>
                            <CardTitle
                                title={task.title}
                                creatorId={task.creatorId}
                                assignedUserId={task.assignedUserId}
                                priority={task.priority}
                                status={task.status}
                                startDate={new Date(task.startDate)}
                                endDate={new Date(task.endDate)}
                                users={users}
                                userRole={userRole}
                            />
                        </>}

                        bordered={false}
                    >
                        <p>Комментарий: </p>
                        <p style={{ maxWidth: 400, wordWrap: 'break-word' }}>{task.comment}</p>

                        <div className="card_buttons">
                            {(userRole === Role.Admin || userRole === Role.Manager) && (
                                <Button onClick={() => handleOpen(task)} style={{ flex: 1 }}>
                                    Редактировать
                                </Button>
                            )}

                            {userRole === Role.Admin && (
                                <Button
                                    onClick={() => handleDelete(task.id)}
                                    danger
                                    style={{ flex: 1, marginLeft: 8 }}
                                >
                                    Удалить
                                </Button>
                            )}

                            {userRole === Role.Performer && (
                                task.status === 1 ? (
                                    <Button onClick={() => handleAcceptTask(task.id)} style={{ flex: 1 }}>
                                        Принять задачу
                                    </Button>
                                ) : task.status === 2 ? (
                                    <Button onClick={() => handleAcceptTask(task.id)} style={{ flex: 1 }}>
                                        Завершить задачу
                                    </Button>
                                ) : null
                            )}
                            <Button onClick={() => showHistory(task.id)}>История изменений</Button>
                        </div>
                    </Card>
                )
            ))}
        </div>
    );
};
