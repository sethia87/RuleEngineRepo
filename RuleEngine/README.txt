- Briefly describe the conceptual approach you chose! What are the trade-offs?
  
  I have used WPF for GUI, MVVM pattren and C# for language.
  Trade-offs: can only be run in desktop and not on web( hence not accessible from anywhere), need to be installed on your system 

- What's the runtime performance? What is the complexity? Where are the bottlenecks?
  runtime performance is good, complexity is O(n^2), Bottleneck is parsing of json string.

- If you had more time, what improvements would you make, and in what order of priority?
  Improvements I would like to have as follows :
  1. Create a WCF service for desktop application or web api service for web application
  2. Would like to smoothen json parsing
  3. Create a more rich UI