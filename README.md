# RBC-GAM
Design and implement a class or classes to monitor the changing price of a financial instrument using C# (you can use another modern programming language, but keep in mind that teams at RBC GAM use C#). Users of the class(es) will be responsible for setting prices. Your implementation will provide the following monitoring functionality:

1.    Allow users to update the current price periodically.

2.    Triggers: It must be possible for callers of the class to provide arbitrary thresholds such a “Buy trigger” and “Sell trigger” at which the class will inform subscribers that a threshold has been reached. Triggers should have the following features:

a.    It should be possible to control the sensitivity of triggers so callers are not repeatedly informed of hits if the price is hovering around the trigger point. For example, consider the following price movements:

14.30
14.27
14.26
14.25
14.26
14.25
14.26
14.24
14.25
14.35
14.40

Some callers may only want to be informed that the price has reached 14.25 once because they consider fluctuations of less than +/- 0.02 to be insignificant. 
 
     b.       It should be possible for callers to specify the direction from which a price threshold hit causes a notification. e.g. Only trigger a “Sell trigger” notification if current price rises to    hit the threshold, not if it drops to hit it from above.
	 
Implement these requirements as you understand them. Feel free to outline assumptions you made when you submit the result.
