function ContactForm({ editingContact, name, surname, 
    phoneNumber, setName, setSurname, setPhoneNumber, 
    addContact, updateContact }) {
    return (
        <div className="mt-6 p-4 border rounded-lg shadow-md bg-gray-100">
            <h2 className="text-xl font-semibold mb-2">
                {editingContact ? 'Редактировать контакт' : 'Добавить контакт'}
            </h2>
            <div className="flex space-x-2">
                <input
                    type="text"
                    placeholder="Имя"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                    className="px-3 py-2 border rounded w-1/3"
                />
                <input
                    type="text"
                    placeholder="Фамилия"
                    value={surname}
                    onChange={(e) => setSurname(e.target.value)}
                    className="px-3 py-2 border rounded w-1/3"
                />
                <input
                    type="text"
                    placeholder="Телефон"
                    value={phoneNumber}
                    onChange={(e) => setPhoneNumber(e.target.value)}
                    className="px-3 py-2 border rounded w-1/3"
                />
                <button
                    onClick={editingContact ? updateContact : addContact}
                    className="px-4 py-2 bg-green-500 text-white rounded"
                >
                    {editingContact ? 'Обновить' : 'Добавить'}
                </button>
            </div>
        </div>
    );
}

export default ContactForm;