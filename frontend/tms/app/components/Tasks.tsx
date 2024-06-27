import Card from "antd/es/card/Card";
import { CardTitle } from "./Cardtitle";
import Button from "antd/es/button/button";
import { useEffect, useState } from "react";
import { getAllUsers } from "../services/tasks";

interface Props {
    tasks: Task[];
    handleDelete: (id: string) => void;
    handleOpen: (task: Task) => void;
}

const priorityOrder: Record<string, number> = {
    "Низкий": 1,
    "Средний": 2,
    "Высокий": 3
};

const sortTasks = (tasks: Task[]) => {
    return tasks.sort((a, b) => {

        const priorityComparison = priorityOrder[b.priority] - priorityOrder[a.priority];
        if (priorityComparison !== 0) return priorityComparison;

        return new Date(a.endDate).getTime() - new Date(b.endDate).getTime();
    });
};

export const Tasks = ({ tasks, handleDelete, handleOpen }: Props) => {
    const sortedTasks = sortTasks(tasks);
    const [users, setUsers] = useState<{ userId: string; name: string }[]>([]);
    useEffect(() => {

        const getUsers = async () => {
            try {
                const usersData = await getAllUsers();
                
                setUsers(usersData);

            } catch (error) {
                console.error("Error fetching users:", error);
            }
        };

        getUsers();

        
    }, []);

    return (
        <div className="cards">
            {sortedTasks.map((task: Task) => (
                <Card
                    key={task.id}
                    title={<CardTitle
                        title={task.title}
                        description={task.comment}
                        assignedUserId={task.assignedUserId}
                        priority={task.priority}
                        status={task.status}
                        startDate={task.startDate}
                        endDate={task.endDate} 
                        users={users} 
                        />}
                    bordered={false}
                >
                     <p>Комментарий: </p>
                     <p style={{ maxWidth: 400, wordWrap: 'break-word' }}>{task.comment}</p>

                    <div className="card_buttons">
                        <Button
                            onClick={() => handleOpen(task)}
                            style={{ flex: 1 }}
                        >
                            Редактировать
                        </Button>
                        <Button
                            onClick={() => handleDelete(task.id)}
                            danger
                            style={{ flex: 1 }}
                        >
                            Удалить
                        </Button>
                    </div>
                </Card>
            ))}
        </div>
    );
};
