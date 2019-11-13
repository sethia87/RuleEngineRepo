using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RuleEngine.Model
{
    internal class RuleEngineModel
    {
        #region [Properties]
        public string Signal { private get; set; }
        public string Value { private get; set; }
        public string ValueType { private get; set; }
        public string SelectedCondition { private get; set; }
        #endregion

        #region [Methods]
        public string GetDataThatViolatesRule(string streamingData)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(Signal))
            {
                MessageBox.Show("Please enter the Signal value in create rules");
                flag = true;
            }
            if (string.IsNullOrEmpty(Value))
            {
                MessageBox.Show("Please enter the Value's value in create rules");
                flag = true;
            }
            if (string.IsNullOrEmpty(ValueType))
            {
                MessageBox.Show("Please enter the ValueType value in create rules");
                flag = true;
            }
            if (string.IsNullOrEmpty(streamingData))
            {
                MessageBox.Show("Json txt file is not loaded");
                flag = true;
            }

            if (flag) return string.Empty;

            string result = string.Empty;
            var data = GetListOfData(streamingData);
            var resultFilteredBySignal = data.Where(j => j.Signal == Signal).ToList();
            var resultFilteredByValueType = resultFilteredBySignal.Where(json => json.ValueType.ToLower() == ValueType.ToLower()).ToList();
            List<RuleEngineModel> ruleEngineModelList = null;

            if (ValueType.ToLower() == "datetime")
            {
                switch (SelectedCondition)
                {
                    case "<=":
                        ruleEngineModelList = resultFilteredByValueType.Where(json => Convert.ToDateTime(Value) <= Convert.ToDateTime(json.Value)).ToList();
                        break;
                    case ">=":
                        ruleEngineModelList = resultFilteredByValueType.Where(json => Convert.ToDateTime(Value) >= Convert.ToDateTime(json.Value)).ToList();
                        break;
                    case "=":
                        ruleEngineModelList = resultFilteredByValueType.Where(json => Convert.ToDateTime(Value) == Convert.ToDateTime(json.Value)).ToList();
                        break;
                    case "!=":
                        ruleEngineModelList = resultFilteredByValueType.Where(json => Convert.ToDateTime(Value) != Convert.ToDateTime(json.Value)).ToList();
                        break;
                    case "<":
                        ruleEngineModelList = resultFilteredByValueType.Where(json => Convert.ToDateTime(Value) < Convert.ToDateTime(json.Value)).ToList();
                        break;
                    case ">":
                        ruleEngineModelList = resultFilteredByValueType.Where(json => Convert.ToDateTime(Value) > Convert.ToDateTime(json.Value)).ToList();
                        break;
                }
            }
            else if (ValueType.ToLower() == "integer")
            {
                switch (SelectedCondition)
                {
                    case "<=":
                        ruleEngineModelList = resultFilteredByValueType.Where(json => Convert.ToDouble(Value) <= Convert.ToDouble(json.Value)).ToList();
                        break;
                    case ">=":
                        ruleEngineModelList = resultFilteredByValueType.Where(json => Convert.ToDouble(Value) >= Convert.ToDouble(json.Value)).ToList();
                        break;
                    case "=":
                        ruleEngineModelList = resultFilteredByValueType.Where(json => Convert.ToDouble(Value) == Convert.ToDouble(json.Value)).ToList();
                        break;
                    case "!=":
                        ruleEngineModelList = resultFilteredByValueType.Where(json => Convert.ToDouble(Value) != Convert.ToDouble(json.Value)).ToList();
                        break;
                    case "<":
                        ruleEngineModelList = resultFilteredByValueType.Where(json => Convert.ToDouble(Value) < Convert.ToDouble(json.Value)).ToList();
                        break;
                    case ">":
                        ruleEngineModelList = resultFilteredByValueType.Where(json => Convert.ToDouble(Value) > Convert.ToDouble(json.Value)).ToList();
                        break;
                }
            }
            else
            {
                switch (SelectedCondition)
                {
                    case "=":
                        ruleEngineModelList = resultFilteredByValueType.Where(json => Value == json.Value).ToList();
                        break;
                    case "!=":
                        ruleEngineModelList = resultFilteredByValueType.Where(json => Value != json.Value).ToList();
                        break;
                }
            }
            if (ruleEngineModelList != null)
            {
                foreach (var item in ruleEngineModelList)
                {
                    var r = data.Find(x => x.Signal == item.Signal && x.Value == item.Value && x.ValueType == item.ValueType);
                    if (r != null)
                        data.Remove(r);

                }
                foreach (var res in data)
                {
                    //result += "{" + "'signal' :" + " '" + res.Signal + "', " + "'value' :" + " '" + res.Value +
                              //"', " + "'value_type' :" + " '" + res.ValueType + "'}" + Environment.NewLine;
                    result += res.Signal + Environment.NewLine;
                }
            }
            return result;
        }

        private List<RuleEngineModel> GetListOfData(string streamingData)
        {
            streamingData = streamingData.Remove(0, 1);
            streamingData = streamingData.Remove(streamingData.Length - 1, 1);
            var r1 = streamingData.Split(new char[] { '}' });

            string[] r3;
            List<RuleEngineModel> ruleEngineModelList = new List<RuleEngineModel>();

            for (int i = 0; i < r1.Length; i++)
            {
                if (!string.IsNullOrEmpty(r1[i]))
                {
                    r1[i] = r1[i].TrimStart(',');
                    r1[i] = r1[i].TrimEnd('"');

                    var r2 = r1[i].Split(',');
                    RuleEngineModel json = new RuleEngineModel();

                    for (int j = 0; j < r2.Length; j++)
                    {
                        r2[j] = r2[j].Trim();
                        if (j == 2 && r2[1].Contains("Datetime"))
                        {

                            r3 = r2[2].Split(' ');
                            r3[0] = r3[0].Remove(r3[0].Length - 1, 1);
                            r3[1] = r3[1].Remove(0, 1);

                            string dateValue = r3[1] + " " + r3[2];
                            r3[1] = dateValue;
                        }
                        else
                        {
                            r2[j] = r2[j].Trim();
                            r2[j] = r2[j].TrimStart('{');
                            r3 = r2[j].Split(':');
                        }

                        if (r3.Any() && !string.IsNullOrEmpty(r3[0]))
                        {
                            //r3[0] = r3[0].TrimStart('{');
                            //r3[0] = r3[0].TrimStart('{');
                            r3[0] = r3[0].Trim();
                            r3[0] = r3[0].TrimStart('"');
                            r3[0] = r3[0].TrimEnd('"');
                        }
                        if (r3.Count() > 1 && !string.IsNullOrEmpty(r3[1]))
                        {
                            r3[1] = r3[1].Trim();
                            r3[1] = r3[1].TrimStart('"');
                            r3[1] = r3[1].TrimEnd('"');
                        }

                        r3[0] = r3[0].Trim();
                        if (r3[0].Contains("\""))
                            r3[0] = r3[0].Remove(0, 1);


                        r3[1] = r3[1].TrimStart(' ');

                        if (r3[1].Contains("\""))
                            r3[1] = r3[1].Remove(0, 1);

                        switch (r3[0])
                        {
                            case "signal":
                                json.Signal = r3[1];
                                break;

                            case "value":
                                json.Value = r3[1];
                                break;

                            case "value_type":
                                json.ValueType = r3[1];
                                break;
                        }
                    }
                    ruleEngineModelList.Add(json);
                }
            }
            return ruleEngineModelList;
        }
        #endregion
    }
}
