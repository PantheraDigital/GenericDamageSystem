using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DamageSysDemo_CSharp
{
    public class StrikeDataComposite : StrikeData
    {
        public class Container
        {
            List<StrikeData> dataList;

            public Container(List<StrikeData> list = null)
            {
                if (list == null)
                    dataList = new List<StrikeData>();
                else
                {
                    dataList = list;
                    dataList.Sort();
                }
            }

            public Container Clone()
            {
                List<StrikeData> cloneList = new List<StrikeData>();
                foreach(StrikeData data in dataList)
                {
                    cloneList.Add(data.Clone());
                }
                return new Container(cloneList);
            }

            public void AddData(StrikeData data)
            {
                dataList.Add(data);
                dataList.Sort();
            }
            public void AddData(List<StrikeData> dataList)
            {
                foreach(StrikeData data in dataList)
                {
                    this.dataList.Add(data);
                }
                this.dataList.Sort();
            }

            public bool HasData(string id) 
            {
                return BinarySearch(dataList, id) >= 0;
            }

            public List<StrikeData> GetData()
            {
                List<StrikeData> result = new List<StrikeData>(dataList.Count);
                foreach(StrikeData data in dataList)
                {
                    result.Add(data);
                }
                return result;
            }
            public StrikeData GetData(string id) 
            {
                int index = BinarySearch(dataList, id);
                while(index > 0 && dataList[index - 1].ID == id)
                {
                    index--;
                }
                if (index == -1)
                    return null;

                return dataList[index];
            }

            public Container SubComposite(string[] ids)
            {
                if (ids == null || ids.Length == 0)
                    return null;

                List<StrikeData> result = new List<StrikeData>();
                foreach (string id in ids)
                {
                    List<StrikeData> temp = GetAllMatchingIDs(id);
                    if (temp != null)
                        result = result.Concat(temp).ToList();
                }
                return new Container(result);
            }
            public Container SubComposite(string id) 
            {
                return new Container(GetAllMatchingIDs(id));
            }
            public Container SubCompositeExclude(string id)
            {
                List<StrikeData> result = new List<StrikeData>();
                foreach(StrikeData data in dataList)
                {
                    if (data.ID != id)
                        result.Add(data);
                }

                return (result.Count == 0) ? null : new Container(result);
            }
            
            public void Clear()
            {
                dataList.Clear();
            }
            
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                for(int i = 0; i < dataList.Count; ++i)
                {
                    sb.Append("{" + dataList[i].ToString() + "}");
                    if (i + 1 < dataList.Count)
                        sb.Append("; ");
                }
                return sb.ToString();
            }

            List<StrikeData> GetAllMatchingIDs(string id)
            {
                List<StrikeData> result = new List<StrikeData>();
                int index = FindFirst(dataList, id);
                if (index == -1)
                    return null;

                do
                {
                    result.Add(dataList[index]);
                    index++;
                }
                while (index < dataList.Count && dataList[index].ID == id);
                return result;
            }

            static int BinarySearch(List<StrikeData> inputArray, string key)
            {
                int min = 0;
                int max = inputArray.Count - 1;
                while (min <= max)
                {
                    int mid = (min + max) / 2;
                    int result = key.CompareTo(inputArray[mid].ID);
                    if (result == 0)
                    {
                        return ++mid;
                    }
                    else if (result < 0)
                    {
                        max = mid - 1;
                    }
                    else
                    {
                        min = mid + 1;
                    }
                }
                return -1;
            }
            static int FindFirst(List<StrikeData> inputArray, string id)
            {
                int index = BinarySearch(inputArray, id);
                while (index > 0 && inputArray[index - 1].ID == id)
                {
                    index--;
                }
                return index;
            }
        }

        public Container ToReceiver { get; private set; }
        public Container ToAttacker { get; private set; }


        public StrikeDataComposite(string id) 
            : base(id)
        {
            ToReceiver = new Container();
            ToAttacker = new Container();
        }
        public StrikeDataComposite(string id, Container toReceiver, Container toAttacker)
            : base(id)
        {
            ToReceiver = toReceiver;
            ToAttacker = toAttacker;
        }
        public override StrikeData Clone()
        {
            return new StrikeDataComposite(ID, ToReceiver.Clone(), ToAttacker.Clone());
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("StrikeDataComposite - {0}\n\tToAttacker: {1}\n\tToReceiver: {2}", ID, ToAttacker.ToString(), ToReceiver.ToString());
            return sb.ToString();
        }
    }
}
