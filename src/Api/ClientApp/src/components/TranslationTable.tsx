import React from 'react';
import { JobStatus, TranslationJobDto } from '../api/clientApi';

interface TranslationTableProps {
  onRowSelected: (job: TranslationJobDto) => void;
  data: TranslationJobDto[];
}

const TranslationTable: React.FC<TranslationTableProps> = ({ data, onRowSelected: onJobSelected }) => {
  // const [customerName, setName] = useState('');

  return (
    <>
    <table>
      <thead>
        <tr>
          <th>ID</th>
          <th>Customer Name</th>
          <th>Status</th>
          <th>Original Content</th>
          <th>Translated Content</th>
          <th>Price</th>
          <th>Translator ID</th>
        </tr>
      </thead>
      <tbody>
        {data.map((job) => (
          <tr key={job.id} onClick={() => onJobSelected(job)} className='pointer'>
            <td>{job.id}</td>
            <td>{job.customerName}</td>
            <td>{JobStatus[job.status]}</td>
            <td>{job.originalContent}</td>
            <td>{job.translatedContent}</td>
            <td>{job.price}</td>
            <td>{job.translatorId}</td>
          </tr>
        ))}
      </tbody>
    </table>
      
    </>
  );
};

export default TranslationTable;
