import React, { useState, useEffect } from 'react';
import Pagination from './components/Pagination/Pagination.jsx';
import ContactTable from './components/ContactTable/ContactTable.jsx';
import ContactHeader from './components/ContactHeader/ContactHeader.jsx';
import ContactForm from './components/ContactForm/ContactForm.jsx';

function App() {
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
      <ContactHeader/>
      <ContactTable
        contacts={contacts}
        editContact={editContact}
        deleteContact={deleteContact}
      />

      <Pagination
        page={page}
        setPage={setPage}
      />

      <ContactForm
        editingContact={editingContact}
        name={name}
        surname={surname}
        phoneNumber={phoneNumber}
        setName={setName}
        setSurname={setSurname}
        setPhoneNumber={setPhoneNumber}
        addContact={addContact}
        updateContact={updateContact}
      />
    </div>
  );
}

export default App;