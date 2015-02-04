using UnityEngine;
using System.Collections;

public class iTweenLite
{	
	public enum EaseType{
		easeInQuad = 0,
		easeOutQuad,
		easeInOutQuad,
		easeInCubic,
		easeOutCubic,
		easeInOutCubic,
		easeInQuart,
		easeOutQuart,
		easeInOutQuart,
		easeInQuint,
		easeOutQuint,
		easeInOutQuint,
		easeInSine,
		easeOutSine,
		easeInOutSine,
		easeInExpo,
		easeOutExpo,
		easeInOutExpo,
		easeInCirc,
		easeOutCirc,
		easeInOutCirc,
		linear,
		spring,
		/* GFX47 MOD START */
		//bounce,
		easeInBounce,
		easeOutBounce,
		easeInOutBounce,
		/* GFX47 MOD END */
		easeInBack,
		easeOutBack,
		easeInOutBack,
		/* GFX47 MOD START */
		//elastic,
		easeInElastic,
		easeOutElastic,
		easeInOutElastic,
		/* GFX47 MOD END */
		punch
	}

	public delegate float EasingFunction(float start, float end, float value);

	
	//instantiates a cached ease equation refrence:
	static public EasingFunction GetEaseFunc(EaseType easeType)
	{
		switch (easeType)
		{
		case EaseType.easeInQuad:
			return easeInQuad;
			
		case EaseType.easeOutQuad:
			return easeOutQuad;
			
		case EaseType.easeInOutQuad:
			return easeInOutQuad;
			
		case EaseType.easeInCubic:
			return easeInCubic;
			
		case EaseType.easeOutCubic:
			return easeOutCubic;
			
		case EaseType.easeInOutCubic:
			return easeInOutCubic;
			
		case EaseType.easeInQuart:
			return easeInQuart;
			
		case EaseType.easeOutQuart:
			return easeOutQuart;
			
		case EaseType.easeInOutQuart:
			return easeInOutQuart;
			
		case EaseType.easeInQuint:
			return easeInQuint;
			
		case EaseType.easeOutQuint:
			return easeOutQuint;
			
		case EaseType.easeInOutQuint:
			return easeInOutQuint;
			
		case EaseType.easeInSine:
			return easeInSine;
			
		case EaseType.easeOutSine:
			return easeOutSine;
			
		case EaseType.easeInOutSine:
			return easeInOutSine;
			
		case EaseType.easeInExpo:
			return easeInExpo;
			
		case EaseType.easeOutExpo:
			return easeOutExpo;
			
		case EaseType.easeInOutExpo:
			return easeInOutExpo;
			
		case EaseType.easeInCirc:
			return easeInCirc;
			
		case EaseType.easeOutCirc:
			return easeOutCirc;
			
		case EaseType.easeInOutCirc:
			return easeInOutCirc;
			
		case EaseType.linear:
			return linear;
			
		case EaseType.spring:
			return spring;
			
			/* GFX47 MOD START */
			/*case EaseType.bounce:
			return bounce;
			*/
		case EaseType.easeInBounce:
			return easeInBounce;
			
		case EaseType.easeOutBounce:
			return easeOutBounce;
			
		case EaseType.easeInOutBounce:
			return easeInOutBounce;
			
			/* GFX47 MOD END */
		case EaseType.easeInBack:
			return easeInBack;
			
		case EaseType.easeOutBack:
			return easeOutBack;
			
		case EaseType.easeInOutBack:
			return easeInOutBack;
			
			/* GFX47 MOD START */
			/*case EaseType.elastic:
			return elastic;
			*/
		case EaseType.easeInElastic:
			return easeInElastic;
			
		case EaseType.easeOutElastic:
			return easeOutElastic;
			
		case EaseType.easeInOutElastic:
			return easeInOutElastic;
			
			/* GFX47 MOD END */
		}
		
		return null;
	}

	//valu: 0 ~ 1
	static public float Easing(float start, float end, float val, EaseType easeType)
	{
		switch (easeType)
		{
		case EaseType.easeInQuad:
			return easeInQuad(start, end, val);
			
		case EaseType.easeOutQuad:
			return easeOutQuad(start, end, val);
			
		case EaseType.easeInOutQuad:
			return easeInOutQuad(start, end, val);
			
		case EaseType.easeInCubic:
			return easeInCubic(start, end, val);
			
		case EaseType.easeOutCubic:
			return easeOutCubic(start, end, val);
			
		case EaseType.easeInOutCubic:
			return easeInOutCubic(start, end, val);
			
		case EaseType.easeInQuart:
			return easeInQuart(start, end, val);
			
		case EaseType.easeOutQuart:
			return easeOutQuart(start, end, val);
			
		case EaseType.easeInOutQuart:
			return easeInOutQuart(start, end, val);
			
		case EaseType.easeInQuint:
			return easeInQuint(start, end, val);
			
		case EaseType.easeOutQuint:
			return easeOutQuint(start, end, val);
			
		case EaseType.easeInOutQuint:
			return easeInOutQuint(start, end, val);
			
		case EaseType.easeInSine:
			return easeInSine(start, end, val);
			
		case EaseType.easeOutSine:
			return easeOutSine(start, end, val);
			
		case EaseType.easeInOutSine:
			return easeInOutSine(start, end, val);
			
		case EaseType.easeInExpo:
			return easeInExpo(start, end, val);
			
		case EaseType.easeOutExpo:
			return easeOutExpo(start, end, val);
			
		case EaseType.easeInOutExpo:
			return easeInOutExpo(start, end, val);
			
		case EaseType.easeInCirc:
			return easeInCirc(start, end, val);
			
		case EaseType.easeOutCirc:
			return easeOutCirc(start, end, val);
			
		case EaseType.easeInOutCirc:
			return easeInOutCirc(start, end, val);
			
		case EaseType.linear:
			return linear(start, end, val);
			
		case EaseType.spring:
			return spring(start, end, val);
			
			/* GFX47 MOD START */
			/*case EaseType.bounce:
			return bounce(start, end, val);
			*/
		case EaseType.easeInBounce:
			return easeInBounce(start, end, val);
			
		case EaseType.easeOutBounce:
			return easeOutBounce(start, end, val);
		case EaseType.easeInOutBounce:
			return easeInOutBounce(start, end, val);
			/* GFX47 MOD END */
		case EaseType.easeInBack:
			return easeInBack(start, end, val);
		case EaseType.easeOutBack:
			return easeOutBack(start, end, val);
		case EaseType.easeInOutBack:
			return easeInOutBack(start, end, val);
			/* GFX47 MOD START */
			/*case EaseType.elastic:
			return elastic(start, end, val);
			*/
		case EaseType.easeInElastic:
			return easeInElastic(start, end, val);
		case EaseType.easeOutElastic:
			return easeOutElastic(start, end, val);
		case EaseType.easeInOutElastic:
			return easeInOutElastic(start, end, val);
			/* GFX47 MOD END */
		}

		return 0;
	}
	#region Easing Curves
	
	static public float linear(float start, float end, float value){
		return Mathf.Lerp(start, end, value);
	}
	
	static public float clerp(float start, float end, float value){
		float min = 0.0f;
		float max = 360.0f;
		float half = Mathf.Abs((max - min) / 2.0f);
		float retval = 0.0f;
		float diff = 0.0f;
		if ((end - start) < -half){
			diff = ((max - start) + end) * value;
			retval = start + diff;
		}else if ((end - start) > half){
			diff = -((max - end) + start) * value;
			retval = start + diff;
		}else retval = start + (end - start) * value;
		return retval;
	}
	
	static public float spring(float start, float end, float value){
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
		return start + (end - start) * value;
	}
	
	static public float easeInQuad(float start, float end, float value){
		end -= start;
		return end * value * value + start;
	}
	
	static public float easeOutQuad(float start, float end, float value){
		end -= start;
		return -end * value * (value - 2) + start;
	}
	
	static public float easeInOutQuad(float start, float end, float value){
		value /= .5f;
		end -= start;
		if (value < 1) return end / 2 * value * value + start;
		value--;
		return -end / 2 * (value * (value - 2) - 1) + start;
	}
	
	static public float easeInCubic(float start, float end, float value){
		end -= start;
		return end * value * value * value + start;
	}
	
	static public float easeOutCubic(float start, float end, float value){
		value--;
		end -= start;
		return end * (value * value * value + 1) + start;
	}
	
	static public float easeInOutCubic(float start, float end, float value){
		value /= .5f;
		end -= start;
		if (value < 1) return end / 2 * value * value * value + start;
		value -= 2;
		return end / 2 * (value * value * value + 2) + start;
	}
	
	static public float easeInQuart(float start, float end, float value){
		end -= start;
		return end * value * value * value * value + start;
	}
	
	static public float easeOutQuart(float start, float end, float value){
		value--;
		end -= start;
		return -end * (value * value * value * value - 1) + start;
	}
	
	static public float easeInOutQuart(float start, float end, float value){
		value /= .5f;
		end -= start;
		if (value < 1) return end / 2 * value * value * value * value + start;
		value -= 2;
		return -end / 2 * (value * value * value * value - 2) + start;
	}
	
	static public float easeInQuint(float start, float end, float value){
		end -= start;
		return end * value * value * value * value * value + start;
	}
	
	static public float easeOutQuint(float start, float end, float value){
		value--;
		end -= start;
		return end * (value * value * value * value * value + 1) + start;
	}
	
	static public float easeInOutQuint(float start, float end, float value){
		value /= .5f;
		end -= start;
		if (value < 1) return end / 2 * value * value * value * value * value + start;
		value -= 2;
		return end / 2 * (value * value * value * value * value + 2) + start;
	}
	
	static public float easeInSine(float start, float end, float value){
		end -= start;
		return -end * Mathf.Cos(value / 1 * (Mathf.PI / 2)) + end + start;
	}
	
	static public float easeOutSine(float start, float end, float value){
		end -= start;
		return end * Mathf.Sin(value / 1 * (Mathf.PI / 2)) + start;
	}
	
	static public float easeInOutSine(float start, float end, float value){
		end -= start;
		return -end / 2 * (Mathf.Cos(Mathf.PI * value / 1) - 1) + start;
	}
	
	static public float easeInExpo(float start, float end, float value){
		end -= start;
		return end * Mathf.Pow(2, 10 * (value / 1 - 1)) + start;
	}
	
	static public float easeOutExpo(float start, float end, float value){
		end -= start;
		return end * (-Mathf.Pow(2, -10 * value / 1) + 1) + start;
	}
	
	static public float easeInOutExpo(float start, float end, float value){
		value /= .5f;
		end -= start;
		if (value < 1) return end / 2 * Mathf.Pow(2, 10 * (value - 1)) + start;
		value--;
		return end / 2 * (-Mathf.Pow(2, -10 * value) + 2) + start;
	}
	
	static public float easeInCirc(float start, float end, float value){
		end -= start;
		return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
	}
	
	static public float easeOutCirc(float start, float end, float value){
		value--;
		end -= start;
		return end * Mathf.Sqrt(1 - value * value) + start;
	}
	
	static public float easeInOutCirc(float start, float end, float value){
		value /= .5f;
		end -= start;
		if (value < 1) return -end / 2 * (Mathf.Sqrt(1 - value * value) - 1) + start;
		value -= 2;
		return end / 2 * (Mathf.Sqrt(1 - value * value) + 1) + start;
	}
	
	/* GFX47 MOD START */
	static public float easeInBounce(float start, float end, float value){
		end -= start;
		float d = 1f;
		return end - easeOutBounce(0, end, d-value) + start;
	}
	/* GFX47 MOD END */
	
	/* GFX47 MOD START */
	//static public float bounce(float start, float end, float value){
	static public float easeOutBounce(float start, float end, float value){
		value /= 1f;
		end -= start;
		if (value < (1 / 2.75f)){
			return end * (7.5625f * value * value) + start;
		}else if (value < (2 / 2.75f)){
			value -= (1.5f / 2.75f);
			return end * (7.5625f * (value) * value + .75f) + start;
		}else if (value < (2.5 / 2.75)){
			value -= (2.25f / 2.75f);
			return end * (7.5625f * (value) * value + .9375f) + start;
		}else{
			value -= (2.625f / 2.75f);
			return end * (7.5625f * (value) * value + .984375f) + start;
		}
	}
	/* GFX47 MOD END */
	
	/* GFX47 MOD START */
	static public float easeInOutBounce(float start, float end, float value){
		end -= start;
		float d = 1f;
		if (value < d/2) return easeInBounce(0, end, value*2) * 0.5f + start;
		else return easeOutBounce(0, end, value*2-d) * 0.5f + end*0.5f + start;
	}
	/* GFX47 MOD END */
	
	static public float easeInBack(float start, float end, float value){
		end -= start;
		value /= 1;
		float s = 1.70158f;
		return end * (value) * value * ((s + 1) * value - s) + start;
	}
	
	static public float easeOutBack(float start, float end, float value){
		float s = 1.70158f;
		end -= start;
		value = (value / 1) - 1;
		return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
	}
	
	static public float easeInOutBack(float start, float end, float value){
		float s = 1.70158f;
		end -= start;
		value /= .5f;
		if ((value) < 1){
			s *= (1.525f);
			return end / 2 * (value * value * (((s) + 1) * value - s)) + start;
		}
		value -= 2;
		s *= (1.525f);
		return end / 2 * ((value) * value * (((s) + 1) * value + s) + 2) + start;
	}
	
	static public float punch(float amplitude, float value){
		float s = 9;
		if (value == 0){
			return 0;
		}
		if (value == 1){
			return 0;
		}
		float period = 1 * 0.3f;
		s = period / (2 * Mathf.PI) * Mathf.Asin(0);
		return (amplitude * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * 1 - s) * (2 * Mathf.PI) / period));
	}
	
	/* GFX47 MOD START */
	static public float easeInElastic(float start, float end, float value){
		end -= start;
		
		float d = 1f;
		float p = d * .3f;
		float s = 0;
		float a = 0;
		
		if (value == 0) return start;
		
		if ((value /= d) == 1) return start + end;
		
		if (a == 0f || a < Mathf.Abs(end)){
			a = end;
			s = p / 4;
		}else{
			s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
		}
		
		return -(a * Mathf.Pow(2, 10 * (value-=1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
	}		
	/* GFX47 MOD END */
	
	/* GFX47 MOD START */
	//static public float elastic(float start, float end, float value){
	static public float easeOutElastic(float start, float end, float value){
		/* GFX47 MOD END */
		//Thank you to rafael.marteleto for fixing this as a port over from Pedro's UnityTween
		end -= start;
		
		float d = 1f;
		float p = d * .3f;
		float s = 0;
		float a = 0;
		
		if (value == 0) return start;
		
		if ((value /= d) == 1) return start + end;
		
		if (a == 0f || a < Mathf.Abs(end)){
			a = end;
			s = p / 4;
		}else{
			s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
		}
		
		return (a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end + start);
	}		
	
	/* GFX47 MOD START */
	static public float easeInOutElastic(float start, float end, float value){
		end -= start;
		
		float d = 1f;
		float p = d * .3f;
		float s = 0;
		float a = 0;
		
		if (value == 0) return start;
		
		if ((value /= d/2) == 2) return start + end;
		
		if (a == 0f || a < Mathf.Abs(end)){
			a = end;
			s = p / 4;
		}else{
			s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
		}
		
		if (value < 1) return -0.5f * (a * Mathf.Pow(2, 10 * (value-=1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
		return a * Mathf.Pow(2, -10 * (value-=1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
	}		
	/* GFX47 MOD END */
	
	#endregion	

}
