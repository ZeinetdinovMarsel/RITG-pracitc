import { format } from 'date-fns';
import { Role } from '../enums/Role';

interface Props {
    title: string;
    creatorId: string;
    assignedUserId: string;
    priority: number;
    status: number;
    startDate: Date;
    endDate: Date;
    users: { userId: string; name: string }[];
    userRole: Role;
}

export const CardTitle = ({
    title,
    creatorId,
    assignedUserId,
    priority,
    status,
    startDate,
    endDate,
    users,
    userRole
}: Props) => {
    const getUserName = (userId: string) => {
        const user = users.find(user => user.userId === userId);
        return user ? user.name : "Неизвестный пользователь";
    };
    const getStatusText = (status: number) => {
        switch (status) {
            case 1:
                return "Не принят";
            case 2:
                return "В разработке";
            case 3:
                return "Завершена";
            default:
                return "Неизвестный статус";
        }
    };

    const getPriorityText = (status: number) => {
        switch (status) {
            case 1:
                return "Низкий";
            case 2:
                return "Средний";
            case 3:
                return "Высокий";
            default:
                return "Неизвестный приоритет";
        }
    };
    return (
        <div style={{
            display: "flex",
            flexDirection: "column",
            justifyContent: "space-between",
        }}>
            <p className="card_title">Задача: {title}</p>
            {userRole === Role.Admin && (
                <p className="card_title">От пользователя: {getUserName(creatorId)}</p>
            )}
            {userRole !== Role.Performer && (
                <p className="card_title">Пользователю: {getUserName(assignedUserId)}</p>
            )}
            <p className="card_title">Приоритет: {getPriorityText(priority)}</p>
            <p className="card_title">Статус: {getStatusText(status)}</p>
            <p className="card_price">Дата начала: {format(startDate, 'dd.MM.yyyy')}</p>
            <p className="card_price">Дата конца: {format(endDate, 'dd.MM.yyyy')}</p>
        </div>
    );
};
