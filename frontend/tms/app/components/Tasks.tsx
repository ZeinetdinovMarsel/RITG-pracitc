import Card from "antd/es/card/Card"
import { CardTitle } from "./Cardtitle"
import Button from "antd/es/button/button"
interface Props {
    tasks: Task[]
    handleDelete: (id: string) => void;
    handleOpen: (task: Task) => void;
}

export const Tasks = ({ tasks, handleDelete, handleOpen }: Props) => {
    return (
        <div className="cards">
            {tasks.map((task: Task) => (
                <Card key={task.id} title={<CardTitle
                    title={task.title}
                    description={task.description}
                    assignedUserId={task.assignedUserId}
                    priority={task.priority}
                    status={task.status}
                    startDate={task.startDate}
                    endDate={task.endDate} />} bordered={false}>
                    <p>{task.description}</p>
                    <div className="card_buttons">
                        <Button
                            onClick={() => handleOpen(task)}
                            style={{ flex: 1 }}>
                            Редактировать
                        </Button>
                        <Button
                            onClick={() => handleDelete(task.id)}
                            danger
                            style={{ flex: 1 }}>
                            Удалить
                        </Button>
                    </div>
                </Card>
            ))}
        </div>
    )
}
