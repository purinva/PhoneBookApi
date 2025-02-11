import React, { useState, useEffect } from 'react';
import { Edit, Trash } from 'lucide-react';

function ContactsList() {
  const [contacts, setContacts] = useState([]);
  const [page, setPage] = useState(1);
  const pageSize = 10;

  const [name, setName] = useState('');
  const [surname, setSurname] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');

  const [editingContact, setEditingContact] = useState(null);

  const fetchContacts = (pageNumber) => {
    fetch(`http://localhost:5064/api/User?page=${pageNumber}&pageSize=${pageSize}`)
      .then(response => response.json())
      .then(data => {
        setContacts(data.items || data || []);
      })
      .catch(error => {
        console.error('Error fetching contacts:', error);
        setContacts([]);
      });
  };

  useEffect(() => {
    fetchContacts(page);
  }, [page]);

  const addContact = () => {
    const newContact = { name, surname, phoneNumber };

    fetch('http://localhost:5064/api/User', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(newContact),
    })
      .then(response => {
        if (!response.ok) {
          throw new Error('Ошибка при добавлении контакта');
        }
        return response.json();
      })
      .then(() => {
        setName('');
        setSurname('');
        setPhoneNumber('');
        fetchContacts(page);
      })
      .catch(error => console.error('Error adding contact:', error));
  };

  const editContact = (contact) => {
    setEditingContact(contact);
    setName(contact.name);
    setSurname(contact.surname);
    setPhoneNumber(contact.phoneNumber);
  };

  const updateContact = () => {
    if (!editingContact) return;

    const updatedContact = { name, surname, phoneNumber };

    fetch(`http://localhost:5064/api/User/${editingContact.id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(updatedContact),
    })
      .then(response => {
        if (!response.ok) {
          throw new Error('Ошибка при обновлении контакта');
        }
        return response.json();
      })
      .then(() => {
        setEditingContact(null);
        setName('');
        setSurname('');
        setPhoneNumber('');
        fetchContacts(page);
      })
      .catch(error => console.error('Error updating contact:', error));
  };

  const deleteContact = (id) => {
    if (window.confirm('Вы уверены, что хотите удалить этот контакт?')) {
      fetch(`http://localhost:5064/api/User/${id}`, {
        method: 'DELETE',
      })
        .then(response => {
          if (!response.ok) {
            throw new Error('Ошибка при удалении контакта');
          }
          fetchContacts(page);
        })
        .catch(error => console.error('Error deleting contact:', error));
    }
  };

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-3xl font-semibold mb-4">Список телефонных контактов</h1>
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

      <div className="mt-4 flex justify-between items-center">
        <button
          onClick={() => setPage(page - 1)}
          disabled={page === 1}
          className="px-4 py-2 bg-blue-500 text-white rounded disabled:bg-gray-400"
        >
          Назад
        </button>
        <button
          onClick={() => setPage(page + 1)}
          className="px-4 py-2 bg-blue-500 text-white rounded"
        >
          Дальше
        </button>
      </div>

      <div className="mt-6 p-4 border rounded-lg shadow-md bg-gray-100">
        <h2 className="text-xl font-semibold mb-2">{editingContact ? 'Редактировать контакт' : 'Добавить контакт'}</h2>
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
    </div>
  );
}

export default ContactsList;