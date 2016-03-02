package md59db0963bbf397043484a3f6db5cf9cc5;


public class veilederSide
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("PJAPP.veilederSide, PJAPP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", veilederSide.class, __md_methods);
	}


	public veilederSide () throws java.lang.Throwable
	{
		super ();
		if (getClass () == veilederSide.class)
			mono.android.TypeManager.Activate ("PJAPP.veilederSide, PJAPP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
