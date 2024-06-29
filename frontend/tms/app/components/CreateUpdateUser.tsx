import React, { useEffect } from "react";
import { Modal, Form, Input, Select } from "antd";
import { RegisterRequest } from "../services/users";
import { User } from "../Models/User";
import Option from "antd/es/select";
import { Role } from "../enums/Role";

interface CreateUpdateUserProps {
    mode: string;
    values: User;
    isModalOpen: boolean;
    handleCreate: (request: RegisterRequest) => void;
    handleUpdate: (userId: string, request: RegisterRequest) => void;
    handleCancel: () => void;
}

const CreateUpdateUser: React.FC<CreateUpdateUserProps> = ({
    mode,
    values,
    isModalOpen,
    handleCreate,
    handleUpdate,
    handleCancel
}) => {
    const [form] = Form.useForm();

    useEffect(() => {
        form.setFieldsValue(values);
    }, [values]);

    const onFinish = (values: User) => {
        if (mode === "create") {
            handleCreate(values);
        } else {
            handleUpdate(values.userId, values);
        }
    };

    return (
        <Modal
            title={mode === "create" ? "Создать пользователя" : "Обновить пользователя"}
            visible={isModalOpen}
            onOk={() => form.submit()}
            onCancel={handleCancel}
            okText={mode === "create" ? "Создать" : "Обновить"}
            cancelText="Отмена"
        >
            <Form
                form={form}
                onFinish={onFinish}
                layout="vertical"
            >
                {mode === "edit" && (
                    <Form.Item
                        name="userId"
                        label="User ID"
                        rules={[{ required: true, message: "Введите User ID" }]}
                    >
                        <Input disabled />
                    </Form.Item>
                )}
                <Form.Item
                    name="userName"
                    label="Имя пользователя"
                    rules={[{ required: true, message: "Введите имя пользователя" }]}
                >
                    <Input />
                </Form.Item>
                {mode === "create" && (
                    <Form.Item
                        name="password"
                        label="Пароль"
                        rules={[{ required: true, message: "Введите пароль" }]}
                    >
                        <Input.Password />
                    </Form.Item>
                )}
                <Form.Item
                    name="email"
                    label="Email"
                    rules={[{ required: true, message: "Введите email" }]}
                >
                    <Input />
                </Form.Item>
                <Form.Item
                    name="role"
                    label="Роль"
                    rules={[{ required: true, message: "Выберите роль" }]}
                >
                    <Select>
                        <Option value={Role.Admin}>Admin</Option>
                        <Option value={Role.Performer}>Performer</Option>
                        <Option value={Role.Manager}>Manager</Option>
                    </Select>
                </Form.Item>
            </Form>
        </Modal>
    );
};

export default CreateUpdateUser;
