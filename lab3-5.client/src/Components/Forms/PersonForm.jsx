import { useState, useEffect } from 'react';
import Modal from './Modal';
import axios from 'axios';
import './Form.css'

export default function PersonForm({ person, open, onClose }) {
  const [formData, setFormData] = useState({
    FirstName: '',
    LastName: '',
    PhoneNumber: '',
  });

  useEffect(() => {
    if (person) {
        console.log(person)
      setFormData({
        FirstName: person.firstName || '',
        LastName: person.lastName || '',
        PhoneNumber: person.phoneNumber ||'',
      });
    } else {
      setFormData({
        FirstName: '',
        LastName: '',
        PhoneNumber: '',
      });
    }
  }, [person]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    if (person) {
      console.log(formData)
      console.log(person.personId)
      axios.put(`/api/Person/${person.personId}`, formData)
        .then(response => {
          console.log("Person successfully updated:", response);
          onClose();
        })
        .catch(error => {
            if (error.response && error.response.status === 400) {
                const errorMessages = error.response.data.errors;

                let fullErrorMessage = '';
                for (const field in errorMessages) {
                    if (errorMessages.hasOwnProperty(field)) {
                        fullErrorMessage += `${field}: ${errorMessages[field].join(', ')}\n`;
                    }
                }
                alert(fullErrorMessage);
            }
            onClose();
            console.error('', error);
        });
    } else {
        axios.post('/api/Person', formData)
            .then(response => {
                console.log('', response.data);
                onClose();
            })
            .catch(error => {
                if (error.response && error.response.status === 400) {
                    const errorMessages = error.response.data.errors;

                    let fullErrorMessage = '';
                    for (const field in errorMessages) {
                        if (errorMessages.hasOwnProperty(field)) {
                            fullErrorMessage += `${field}: ${errorMessages[field].join(', ')}\n`;
                        }
                    }

                    alert(fullErrorMessage);
                }
                if (error.response && error.response.status === 401) {
                    alert(error.response.data.message + error.response.data.details);
                }
                onClose();
                console.error('', error);
            });
    }
  };


  return (
    <Modal open={open} onClick={onClose}>
        <form className="delivery-form" onSubmit={handleSubmit}>
            <h5 className="form-title">{person ? 'Edit Person Form' : 'Create Person Form'}</h5>

            <div className="form-section">
                <label htmlFor="FirstName" className="form-label">First Name</label>
                <input
                    type="text"
                    className="form-input"
                    id="FirstName"
                    name="FirstName"
                    value={formData.FirstName}
                    onChange={handleChange}
                    maxLength="50"
                    required
                />
            </div>

            <div className="form-section">
                <label htmlFor="LastName" className="form-label">Last Name</label>
                <input
                    type="text"
                    className="form-input"
                    id="LastName"
                    name="LastName"
                    value={formData.LastName}
                    onChange={handleChange}
                    step="1"
                    required
                />
            </div>

            <div className="form-section">
              <label htmlFor="PhoneNumber" className="form-label">Phone Number</label>
              <input
                type="tel"
                className="form-input"
                id="PhoneNumber"
                name="PhoneNumber"
                value={formData.PhoneNumber}
                onChange={handleChange}
                pattern="(\+?\d{1,3}[\s-]?)?(\(?\d{3}\)?[\s-]?)?[\d\s-]{7,10}"
                placeholder="Enter phone number"
                required
              />
              <small className="form-text text-muted">Please enter a valid phone number (e.g., +380994842571).</small>
            </div>

            <div className="form-actions">
                <button type="button" className="btn-secondary" onClick={onClose}>Close</button>
                <button type="submit" className="btn-primary">Save</button>
            </div>
        </form>
    </Modal>

  );
}
