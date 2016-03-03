package md59db0963bbf397043484a3f6db5cf9cc5;


public class Grupperom
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer,
		com.estimote.sdk.BeaconManager.ServiceReadyCallback
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onResume:()V:GetOnResumeHandler\n" +
			"n_onDestroy:()V:GetOnDestroyHandler\n" +
			"n_onServiceReady:()V:GetOnServiceReadyHandler:EstimoteSdk.BeaconManager/IServiceReadyCallbackInvoker, Xamarin.Estimote.Android\n" +
			"";
		mono.android.Runtime.register ("PJAPP.Grupperom, PJAPP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Grupperom.class, __md_methods);
	}


	public Grupperom () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Grupperom.class)
			mono.android.TypeManager.Activate ("PJAPP.Grupperom, PJAPP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onResume ()
	{
		n_onResume ();
	}

	private native void n_onResume ();


	public void onDestroy ()
	{
		n_onDestroy ();
	}

	private native void n_onDestroy ();


	public void onServiceReady ()
	{
		n_onServiceReady ();
	}

	private native void n_onServiceReady ();

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
