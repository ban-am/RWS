import React, { useEffect, useState } from 'react';
import { JobStatus, TranslationJobDto, TranslatorDetailDto, TranslatorDto, TranslatorStatus } from '../api/clientApi';
import clientApi from '../api';

interface TranslatorDetailProps {
  onDeleted: () => void;
  data: TranslatorDto;
}

const TranslatorDetail: React.FC<TranslatorDetailProps> = ({ data, onDeleted }) => {
  const [translator, setTranslator] = useState<TranslatorDetailDto>();
  const [approved, setApproved] = useState<boolean>(false);
  const [jobId, setJobId] = useState<string>();
  const [jobs, setJobs] = useState<TranslationJobDto[]>([]);

  useEffect(() => {
    loadTranslator();
  }, [data.id]);

  const loadTranslator = () => {
    clientApi.translator_GetById(data.id).then(i => {
      if (i.status === TranslatorStatus.Certified) {
        setApproved(true);
        loadJobs();
      }
      setTranslator(i);
    });
  }

  const onApprove = () => {
    clientApi.translator_Approve(data.id).then(i => {
      setApproved(true);
      loadJobs();
      data.status = TranslatorStatus.Certified;
    });
  }

  const onDelete = () => {
    clientApi.translator_Delete(data.id).then(i => onDeleted());
  }

  const loadJobs = () => {
    clientApi.translationJob_GetAll().then(i => setJobs(i.filter(x => !x.translatorId)));
  }

  const assignJob = () => {
    let item = jobs.find(i => `${i.id}` === jobId);
    if (!item) return;
    clientApi.translator_AssignJob(data.id, item.id)
      .then(i =>  loadTranslator())
      .catch(i => console.error(i));
  }

  const setJobStatus = (id: number, status: JobStatus) => {
    clientApi.translationJob_UpdateStatus(id, status);
  }

  return (
    <>
      <div className="attributes">
        <div className="row">
          <span className="w-150">Id:</span>
          <span>{data.id}</span>
        </div>
        <div className="row">
          <span className="w-150">Customer Name:</span>
          <span>{data.name}</span>
        </div>
        <div className="row">
          <span className="w-150">Status:</span>
          <span>{TranslatorStatus[data.status]}</span>
        </div>
        <div className="row">
          <span className="w-150">Hourly Rate:</span>
          <span>{data.hourlyRate}</span>
        </div>

      {translator && <br /> &&  translator.translationJobs?.map(i => (
        <div className="row" key={i.id}>
          <span className="w-150">Job:</span>
          
          <div className="row flex-1">
              <div className="w-100p">
                {i.id} {JobStatus[i.status]}
              </div>
              <div className="w-150">
                  <button disabled={i.status >= JobStatus.InProgress} onClick={() => setJobStatus(i.id, JobStatus.InProgress)} className="w-100p">In progress</button>
                  <button disabled={i.status >= JobStatus.Completed} onClick={() => setJobStatus(i.id, JobStatus.Completed)} className="w-100p">Completed</button>
              </div>
            </div>
        </div>
          
      ))}
      <div className="row">
          <span className="w-150">Assign job:</span>
          
          <div className="row flex-1">
              <div className="w-100p">
                <select className="w-100p" title='Translators' onChange={e => setJobId(e.target.value)}>
                  <option id={`xxx`} value="xxx">Select item...</option>
                  {jobs.map(i => 
                    <option key={i.id} id={`${i.id}`} value={i.id}>{i.id}-{i.customerName}</option>
                  )}
                </select>
              </div>
              <div className="w-150">
                  <button onClick={assignJob} className="w-100p">Assign job</button>
              </div>
            </div>
        </div>
      </div>

      <div className="row">
          <button disabled={approved} onClick={onApprove}>Approve</button>
          <button onClick={onDelete}>Delete</button>
      </div>
    </>
  );
};

export default TranslatorDetail;
