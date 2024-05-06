import React from 'react';
import { useParams } from 'react-router-dom';

function AnnouncementPage({ announcements }) {
  console.log(announcements)
  const { id } = useParams();
  const announcement = announcements.find(item => item.id === parseInt(id));
  

  if (!announcement) {
    return <div>Announcement not found</div>;
  }

  const { student, naam, aangekondigdOp, body } = announcement;

  return (
    <div>
      <h2>{naam}</h2>
      <p><strong>Student:</strong> {student}</p>
      <p><strong>Posted On:</strong> {aangekondigdOp}</p>
      <p>{body}</p>
    </div>
  );
}

export default AnnouncementPage;
