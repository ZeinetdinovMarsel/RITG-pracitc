import { format } from 'date-fns';

interface Props {
    title: string;
    description: string;
    assignedUserId: string;
    priority: string;
    status: string;
    startDate: Date;
    endDate: Date;
    users: { userId: string; name: string }[];
}

export const CardTitle = ({
    title,
    description,
    assignedUserId,
    priority,
    status,
    startDate,
    endDate,
    users
}: Props) => {
    const getUserName = (userId: string) => {
        const user = users.find(user => user.userId === userId);
        return user ? user.name : "Неизвестный пользователь";
    };
    return (
        <div style={{
            display: "flex",
            flexDirection: "column",
            justifyContent: "space-between",
        }}>
            <p className="card_title">Задача: {title}</p>
            <p className="card_title">Назначенный пользователь: {getUserName(assignedUserId)}</p>
            <p className="card_title">Приоритет: {priority}</p>
            <p className="card_title">Статус: {status}</p>
            <p className="card_price">Дата начала: {format(startDate, 'dd.MM.yyyy')}</p>
            <p className="card_price">Дата конца: {format(endDate, 'dd.MM.yyyy')}</p>
        </div>
    );
};
