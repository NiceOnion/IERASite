import React, { useState } from 'react';
import APIAdress from '../Data/APIAddress';

function CreateAnnouncement() {
  const [title, setTitle] = useState('');
  const [content, setContent] = useState('');
  const [selectedImages, setSelectedImages] = useState([]);

  const handleSubmit = (event) => {
    event.preventDefault();
    const announcementData = {
      title: title,
      userId: "0", // Assuming you have userId stored somewhere accessible
      body: content,
      images: selectedImages, // Pass selected images to the backend
    };

    fetch(APIAdress + "/Announcement", {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(announcementData)
    })
    .then(response => {
      if (!response.ok) {
        throw new Error('Failed to create announcement');
      }
      return response.json();
    })
    .then(data => {
      console.log('Announcement created successfully', data);
      // You may want to redirect the user to another page after creating the announcement
    })
    .catch(error => console.error('Error creating announcement:', error));
  };

  const handleImageChange = (event) => {
    const selectedImages = event.target.files;
    setSelectedImages(selectedImages);
  };

  return (
    <div>
      <h1>Create Announcement</h1>
      <form onSubmit={handleSubmit}>
        <label>
          Title:
          <input type="text" value={title} onChange={(e) => setTitle(e.target.value)} />
        </label>
        <br />
        <label>
          Content:
          <textarea value={content} onChange={(e) => setContent(e.target.value)} />
        </label>
        <br />
        <label>
          Images:
          <input type="file" accept="image/*" multiple onChange={handleImageChange} />
        </label>
        <br />
        <button type="submit">Submit</button>
      </form>
    </div>
  );
}

export default CreateAnnouncement;
