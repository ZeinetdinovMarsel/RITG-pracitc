import { format } from 'date-fns';

interface Props {
    title: string;
    description: string;
    assignedUserId: number;
    priority: string;
    status: string;
    startDate: Date;
    endDate: Date;
}

export const CardTitle = ({
    title,
    description,
    assignedUserId,
    priority,
    status,
    startDate,
    endDate
}: Props) => {
    return (
        <div style={{
            display: "flex",
            flexDirection: "column",
            justifyContent: "space-between",
        }}>
            <p className="card_title">Задача: {title}</p>
            <p className="card_title">Назначенный пользователь: {assignedUserId}</p>
            <p className="card_title">Приоритет: {priority}</p>
            <p className="card_title">Статус: {status}</p>
            <p className="card_price">Дата начала: {format(startDate, 'dd.MM.yyyy')}</p>
            <p className="card_price">Дата конца: {format(endDate, 'dd.MM.yyyy')}</p>
        </div>
    );
};
