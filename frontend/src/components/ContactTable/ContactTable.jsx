import { Edit, Trash } from 'lucide-react';

function ContactTable({contacts, editContact, deleteContact}) {
    return (
        <table className="min-w-full table-auto">
            <thead className="bg-gray-200">
            <tr>
                <th className="px-4 py-2 text-left">Id</th>
                <th className="px-4 py-2 text-left">Имя</th>
                <th className="px-4 py-2 text-left">Фамилия</th>
                <th className="px-4 py-2 text-left">Телефон</th>
                <th className="px-4 py-2 text-left"></th>
            </tr>
            </thead>
            <tbody>
            {contacts.map(contact => (
                <tr key={contact.id} className="border-b hover:bg-gray-100">
                <td className="px-4 py-2">{contact.id}</td>
                <td className="px-4 py-2">{contact.name}</td>
                <td className="px-4 py-2">{contact.surname}</td>
                <td className="px-4 py-2">{contact.phoneNumber}</td>
                <td className="px-4 py-2 flex space-x-2">
                    <button
                    onClick={() => editContact(contact)}
                    className="text-yellow-500 hover:text-yellow-700"
                    >
                    <Edit size={20} />
                    </button>

                    <button
                    onClick={() => deleteContact(contact.id)}
                    className="text-red-500 hover:text-red-700"
                    >
                    <Trash size={20} />
                    </button>
                </td>
                </tr>
            ))}
            </tbody>
        </table>
    );
}
export default ContactTable;