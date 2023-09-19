import React from 'react';
import { TranslatorDto, TranslatorStatus } from '../api/clientApi';

interface TranslatorTableProps {
  onRowSelected: (item: TranslatorDto) => void;
  data: TranslatorDto[];
}

const TranslatorTable: React.FC<TranslatorTableProps> = ({ data, onRowSelected }) => {
  return (
    <>
    <table>
      <thead>
        <tr>
          <th>ID</th>
          <th>Name</th>
          <th>Status</th>
          <th>Hourly Rate</th>
        </tr>
      </thead>
      <tbody>
        {data.map((i) => (
          <tr key={i.id} onClick={() => onRowSelected(i)} className='pointer'>
            <td>{i.id}</td>
            <td>{i.name}</td>
            <td>{TranslatorStatus[i.status]}</td>
            <td>{i.hourlyRate}</td>
          </tr>
        ))}
      </tbody>
    </table>
      
    </>
  );
};

export default TranslatorTable;
