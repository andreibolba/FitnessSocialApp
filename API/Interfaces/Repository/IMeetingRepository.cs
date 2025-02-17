﻿using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IMeetingRepository
    {
        MeetingDto Create(MeetingDto meeting);
        bool CreateUpdateDelete(string obj, int meetingId, int op);
        IEnumerable<MeetingDto> GetAll();
        IEnumerable<MeetingDto> GetFutureMeetings();
        IEnumerable<MeetingDto> GetAllByInternId(int internId, int? count = null);
        IEnumerable<MeetingDto> GetAllByGroupId(int groupId, int? count = null);
        IEnumerable<MeetingDto> GetAllByTrainerId(int trainerId, int? count = null);
        IEnumerable<T> GettAllChecked<T>(int meetingId,int? trainerId=null);
        MeetingDto GetMeetingById(int id);
        MeetingDto Update(MeetingDto meeting);
        void Delete(int id);
        bool SaveAll();
    }
}
